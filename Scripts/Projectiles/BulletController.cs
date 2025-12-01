using Microlight.MicroBar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class BulletController : MonoBehaviour
{
    protected bool isInitialized;
    protected bool isHit;

    protected new Rigidbody rigidbody;
    protected Vector3 prevPos;
    protected float bulletRadius;

    protected float damage;
    protected Faction faction;
    protected float moveSpeed;
    protected float hitTime;
    protected float lifeTime;
    protected bool isPenetration;
    protected float currentLifeTime;

    protected Dictionary<UnitController, HitInfo> hitInfoMap = new Dictionary<UnitController, HitInfo>();

    protected class HitInfo
    {
        public int hitCount = 0; // 피격된 횟수
        public bool canHit = true; // 타격 가능 여부
    }

    public virtual void Initialize(float startDamage, float critDamage, float startSpeed, float startLifeTime, bool isStartPenetration, Faction senderFaction, Vector3 size)
    {
        rigidbody = GetComponent<Rigidbody>();
        bulletRadius = GetComponent<CapsuleCollider>().radius;

        damage = startDamage * critDamage;
        moveSpeed = startSpeed;
        lifeTime = startLifeTime;
        isPenetration = isStartPenetration;
        faction = senderFaction;

        transform.localScale = size;
        prevPos = transform.position;

        isHit = false;
        isInitialized = true;
    }

    protected virtual void CheckHit(UnitController target)
    {
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
