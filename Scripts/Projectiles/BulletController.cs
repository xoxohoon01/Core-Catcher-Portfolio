using DamageNumbersPro;
using Microlight.MicroBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class BulletController : MonoBehaviour
{
    public static Action OnPlayerHit;

    protected DamageNumber damageNumberPrefab;

    protected bool isInitialized;
    protected bool isHit;

    protected new Rigidbody rigidbody;
    protected Vector3 prevPos;
    protected float bulletRadius;

    protected CritResult critResult;
    protected float rawDamage; // 크리티컬 적용 전 기본 데미지
    protected float damage;
    private bool hasFiredCritEvent; // 이 인스턴스에서 크리티컬 이벤트를 이미 발생시켰는지 여부
    public bool countAsAttackHit = true;   // AfterBurner 카운트에 포함할지 여부
    public bool triggersBulletHit = true;  // FlyingBullet · Targeting 발동 여부
    protected float moveSpeed;
    protected float hitTime;
    protected float lifeTime;
    protected bool isPenetration;
    protected float currentLifeTime;
    protected UnitController sender;
    protected Faction faction;

    protected Dictionary<UnitController, HitInfo> hitInfoMap = new Dictionary<UnitController, HitInfo>();

    protected class HitInfo
    {
        public int hitCount = 0; // 피격된 횟수
        public bool canHit = true; // 타격 가능 여부
    }

    public virtual void Initialize(float baseDamage, int critLevel, float startSpeed, float startLifeTime, bool isStartPenetration, UnitController sender, Faction senderFaction, Vector3 size)
    {
        this.sender = sender;

        rigidbody = GetComponent<Rigidbody>();
        bulletRadius = GetComponent<CapsuleCollider>().radius;

        critResult = CriticalCalculator.Calculate(sender);

        damageNumberPrefab = critResult.damageNumber;

        rawDamage = baseDamage;
        damage = baseDamage * critResult.multiplier;
        moveSpeed = startSpeed;
        lifeTime = startLifeTime;
        isPenetration = isStartPenetration;
        faction = senderFaction;

        transform.localScale = size;
        prevPos = transform.position;

        isHit = false;
        countAsAttackHit = true;
        isInitialized = true;
    }

    protected virtual void CheckHit(UnitController target)
    {
        float crit = target.status.critChance;

        float finalDamage = damage * critResult.multiplier;

        // 방어력 계산
        float armor = target.status.armor;
        float damageReduction = armor / (armor + 300f);
        finalDamage = finalDamage * (1f - damageReduction);

        float remainDamage = finalDamage;

        if (target.status.shield > 0)
        {
            if (remainDamage <= target.status.shield)
            {
                target.status.shield -= remainDamage;
                DamageNumberManager.Instance.shieldHitPrefab.Spawn(target.transform.position, remainDamage);
                remainDamage = 0;
            }
            else
            {
                float shieldDamage = target.status.shield;

                target.status.shield = 0;

                DamageNumberManager.Instance.shieldHitPrefab
                    .Spawn(target.transform.position, shieldDamage);

                remainDamage -= shieldDamage;
            }

            if (target.faction == Faction.Player)
            {
                ScreenEffectManager.Instance.BeginEffect("Shield");
            }
        }
        if (remainDamage > 0)
        {
            target.status.hp -= remainDamage;
            damageNumberPrefab.Spawn(target.transform.position, remainDamage);

            if (target.faction == Faction.Player)
            {
                ScreenEffectManager.Instance.BeginEffect("Damage");
                OnPlayerHit?.Invoke();
            }

            // 플레이어의 공격이 적에게 명중했을 때 이벤트 발생
            // InvokeCritical은 이 BulletController 인스턴스당 1회만 — 관통 시 중복 방지
            if (sender is PlayerController playerSender)
            {
                if (countAsAttackHit)
                    playerSender.InvokeAttackHit(target, target.transform.position);
                if (triggersBulletHit)
                    playerSender.InvokeBulletHit(target, target.transform.position);
                if (!hasFiredCritEvent)
                {
                    hasFiredCritEvent = true;
                    playerSender.InvokeCritical(critResult.level);
                }
            }
        }
    }

    protected virtual void AfterHit(UnitController target)
    {
        if (target.healthBar != null)
            target.healthBar.UpdateBar(target.status.hp, false, UpdateAnim.Damage);
    }

    protected IEnumerator CoRestoreMultiHit(UnitController target, float time)
    {
        yield return new WaitForSeconds(time);
        hitInfoMap[target].canHit = true;
    }

    private void OnDisable()
    {
        currentLifeTime = 0;
        hitInfoMap.Clear();
        isInitialized = false;
        hasFiredCritEvent = false;
        countAsAttackHit = true;
        triggersBulletHit = true;
    }

    private void Update()
    {
        if (BattleManager.Instance.isStop) return;

        if (currentLifeTime >= lifeTime)
        {
            ObjectPoolManager.Instance.Despawn(gameObject);
        }

        currentLifeTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (BattleManager.Instance.isStop) return;

        Move();
    }

    protected virtual void Move()
    {
        HandleMove(transform.position + (transform.forward * moveSpeed * Time.fixedDeltaTime));
    }

    protected void HandleMove(Vector3 targetVector)
    {
        // 지면 높이
        float groundHeight = GetGroundHeight(targetVector);
        float bulletHeight = groundHeight + 2f; // 지면 위 2

        Vector3 newPos = targetVector;
        //prevPos.y = bulletHeight;
        //newPos.y = bulletHeight;

        Vector3 dir = (newPos - prevPos).normalized;
        float distance = Vector3.Distance(prevPos, newPos);

        RaycastHit[] hits = Physics.SphereCastAll(
        prevPos,
        bulletRadius,
        dir,
        distance,
        LayerMask.GetMask("Player", "Monster", "Ground"),
        QueryTriggerInteraction.Ignore
        );

        if (hits.Length > 0)
        {
            // 가장 가까운 충돌체 찾기
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                if (hit.collider == null)
                    continue;

                int layer = hit.collider.gameObject.layer;

                // 1) Ground 레이어와 충돌 → 바로 사라짐
                if (layer == LayerMask.NameToLayer("Ground"))
                {
                    ObjectPoolManager.Instance.Despawn(gameObject);
                    return;
                }

                if (isPenetration)
                {
                    HandleCollision(hit.collider, hit.point);
                }
                else
                {
                    if (!isHit)
                    {
                        if (HandleCollision(hit.collider, hit.point))
                        {
                            isHit = true;
                        }
                    }
                }
            }

            if (!isPenetration && isHit)
            {
                ObjectPoolManager.Instance.Despawn(gameObject);
            }
        }

        // 실제 물리 이동
        rigidbody.Move(newPos, transform.rotation);
        prevPos = newPos;
    }

    private float GetGroundHeight(Vector3 pos)
    {
        RaycastHit hit;

        // 위에서 아래로 레이 쏨 (높이 50 기준)
        if (Physics.Raycast(pos + Vector3.up * 50f, Vector3.down, out hit, 100f, LayerMask.GetMask("Ground")))
        {
            return hit.point.y;
        }

        // 만약 "Ground" 레이어가 없다면 Terrain 사용
        if (Terrain.activeTerrain != null)
        {
            return Terrain.activeTerrain.SampleHeight(pos);
        }

        // 아무 것도 없으면 기본값(0)
        return 0f;
    }

    private bool HandleCollision(Collider other, Vector3 hitPoint)
    {
        UnitController target = other.GetComponent<UnitController>();
        if (target != null)
        {
            if (target.faction != faction)
            {
                if (!hitInfoMap.ContainsKey(target))
                {
                    HitInfo newHitInfo = new HitInfo();
                    newHitInfo.canHit = false;
                    hitInfoMap.Add(target, newHitInfo);
                    CheckHit(target);
                    AfterHit(target);
                }
                else
                {
                    if (hitInfoMap[target].canHit)
                    {
                        hitInfoMap[target].canHit = false;
                        CheckHit(target);
                        AfterHit(target);
                    }
                }

                return true;
            }
        }

        return false;
    }
}
