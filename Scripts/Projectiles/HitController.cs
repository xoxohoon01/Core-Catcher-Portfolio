using DamageNumbersPro;
using Microlight.MicroBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HitController : MonoBehaviour
{
    // 플레이어가 실제로 피격됐을 때 호출 — 피격된 PlayerController를 인자로 전달
    public static Action<PlayerController> OnPlayerHit;

    protected DamageNumber damageNumberPrefab;
    public GameObject particleObject;
    public GameObject audioObject;

    public bool isRound;
    protected Vector3 centerPos;
    protected Vector3 forwardDir;
    protected float radius;
    protected float innerRadius;
    protected float angle;

    protected bool isInitialized;
    protected new Rigidbody rigidbody;

    protected float damage;
    protected float moveSpeed;
    protected float hitTime;
    protected float startDelay;
    protected float lifeTime;
    protected float currentLifeTime;
    protected float multiHitDelay;
    protected UnitController sender;
    protected Faction faction;

    protected Dictionary<UnitController, HitInfo> hitInfoMap = new Dictionary<UnitController, HitInfo>();

    protected class HitInfo
    {
        public int hitCount = 0; // 피격된 횟수
        public bool canHit = true; // 타격 가능 여부
    }

    public virtual void Initialize(float baseDamage, float moveSpeed, float hitTime, float startDelay, float lifeTime, float multiHitDelay, UnitController sender, Faction senderFaction, Vector3 size)
    {
        this.sender = sender;

        this.damage = baseDamage;
        this.moveSpeed = moveSpeed;
        this.hitTime = hitTime;
        this.startDelay = startDelay;
        this.lifeTime = lifeTime;
        this.multiHitDelay = multiHitDelay;
        faction = senderFaction;

        transform.localScale = size;
        if (particleObject != null)
        {
            particleObject.transform.SetParent(null);
            ParticleSystem[] allParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in allParticles)
            {
                ps.Stop();
            }
        }
        if (audioObject != null)
        {
            AudioSource[] allAudios = audioObject.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource audio in allAudios)
            {
                audio.Pause();
            }
        }
    }

    public virtual void InitializeRound(
        float baseDamage,
        float moveSpeed,
        float hitTime,
        float startDelay,
        float lifeTime,
        float multiHitDelay,
        UnitController sender,
        Faction senderFaction,
        Vector3 size,
        Vector3 centerPos,
        Vector3 forwardDir,
        float radius,
        float innerRadius,
        float angle)
    {
        this.sender = sender;

        this.isRound = true;

        this.damage = baseDamage;
        this.moveSpeed = moveSpeed;
        this.hitTime = hitTime;
        this.startDelay = startDelay;
        this.lifeTime = lifeTime;
        this.multiHitDelay = multiHitDelay;
        faction = senderFaction;
        transform.localScale = size;

        this.centerPos = centerPos;
        this.forwardDir = forwardDir;
        this.radius = radius;
        this.innerRadius = innerRadius;
        this.angle = angle;

        if (particleObject != null)
        {
            particleObject.transform.SetParent(null);
            ParticleSystem[] allParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in allParticles)
            {
                ps.Stop();
            }
        }
        if (audioObject != null)
        {
            AudioSource[] allAudios = audioObject.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource audio in allAudios)
            {
                audio.Pause();
            }
        }
    }

    protected virtual void CheckHit(UnitController target)
    {
        CritResult critResult = CriticalCalculator.Calculate(sender);

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
            critResult.damageNumber.Spawn(target.transform.position, remainDamage);

            if (target.faction == Faction.Player)
            {
                ScreenEffectManager.Instance.BeginEffect("Damage");

                // 플레이어가 실제로 체력 피해를 입었을 때만 이벤트 발생
                OnPlayerHit?.Invoke(target as PlayerController);
            }

            // 플레이어의 공격이 적에게 명중했을 때 이벤트 발생 (아티팩트 연동)
            if (sender is PlayerController playerSender)
            {
                playerSender.InvokeAttackHit(target, target.transform.position);
                playerSender.InvokeCritical(critResult.level);
            }
        }
    }

    public void CheckHitRound()
    {
        if (!isInitialized || startDelay > 0 || (hitTime != 0 && currentLifeTime > hitTime)) return;

        float actualRadius = (radius * transform.localScale.x) / 2;
        float actualInnerRadius = (innerRadius * transform.localScale.x) / 2;

        // 1. 공격 범위 내의 모든 콜라이더 검사
        Collider[] hits = Physics.OverlapSphere(centerPos, actualRadius);

        foreach (var hit in hits)
        {
            UnitController target = hit.GetComponent<UnitController>();

            // 타겟이 유효하고, 아군이 아닐 때만 처리
            if (target != null && target.faction != faction)
            {
                // 플레이어 대시 무적 체크
                if (target.faction == Faction.Player && target.GetComponent<PlayerController>().dashSpan > 0)
                    continue;

                Vector3 targetPos = target.transform.position;
                Vector3 dirToTarget = (targetPos - centerPos).normalized;
                float distanceToTarget = Vector3.Distance(centerPos, targetPos);

                // 2. 거리 및 각도 판정
                bool isWithinDistance = distanceToTarget >= actualInnerRadius && distanceToTarget <= actualRadius;
                float angleToTarget = Vector3.Angle(forwardDir, dirToTarget);
                bool isWithinAngle = angleToTarget <= angle / 2f;

                if (isWithinDistance && isWithinAngle)
                {
                    // 3. 다중 타격 및 중복 데미지 방지 로직 통합
                    if (!hitInfoMap.ContainsKey(target))
                    {
                        HitInfo newHitInfo = new HitInfo();
                        newHitInfo.canHit = false; // 첫 타격 시 막음
                        hitInfoMap.Add(target, newHitInfo);

                        // 기존의 정교한 피격 로직 호출
                        CheckHit(target);
                        AfterHit(target);

                        // multiHitDelay가 있다면 나중에 다시 타격 가능하게 함
                        if (multiHitDelay != 0)
                            StartCoroutine(CoRestoreMultiHit(target, multiHitDelay));
                    }
                    else if (hitInfoMap[target].canHit)
                    {
                        hitInfoMap[target].canHit = false;
                        CheckHit(target);
                        AfterHit(target);

                        if (multiHitDelay != 0)
                            StartCoroutine(CoRestoreMultiHit(target, multiHitDelay));
                    }
                }
            }
        }
    }

    protected virtual void AfterHit(UnitController target)
    {
        if (target.healthBar != null)
        {
            target.healthBar.UpdateBar(target.status.hp, false, UpdateAnim.Damage);
        }
    }

    private IEnumerator CoRestoreMultiHit(UnitController target, float time)
    {
        yield return new WaitForSeconds(time);
        hitInfoMap[target].canHit = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isInitialized || isRound || (hitTime != 0 && currentLifeTime > hitTime) || startDelay > 0) return;

        if (other.GetComponent<UnitController>() != null)
        {
            UnitController target = other.GetComponent<UnitController>();
            if (target.faction != faction)
            {
                if (target.faction == Faction.Player)
                {
                    if (target.GetComponent<PlayerController>().dashSpan > 0)
                        return;
                }
                if (!hitInfoMap.ContainsKey(target))
                {
                    HitInfo newHitInfo = new HitInfo();
                    newHitInfo.canHit = false;
                    hitInfoMap.Add(target, newHitInfo);
                    CheckHit(target);
                    AfterHit(target);
                    if (multiHitDelay != 0)
                        StartCoroutine(CoRestoreMultiHit(target, multiHitDelay));
                }
                else if (hitInfoMap[target].canHit)
                {
                    hitInfoMap[target].canHit = false;
                    CheckHit(target);
                    AfterHit(target);
                    if (multiHitDelay != 0)
                        StartCoroutine(CoRestoreMultiHit(target, multiHitDelay));
                }
            }
        }
    }

    private void OnDisable()
    {
        currentLifeTime = 0;
        hitInfoMap.Clear();
        isInitialized = false;
    }

    protected virtual void Update()
    {
        if (BattleManager.Instance.isStop) return;

        if (startDelay > 0)
        {
            startDelay -= Time.deltaTime;
        }

        if (startDelay <= 0)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                if (particleObject != null)
                {
                    ParticleSystem[] allParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in allParticles)
                    {
                        ps.Play();
                    }
                }
                if (audioObject != null)
                {
                    AudioSource[] allAudios = audioObject.GetComponentsInChildren<AudioSource>();
                    foreach (AudioSource audio in allAudios)
                    {
                        audio.Play();
                    }
                }
            }

            if (isRound)
            {
                CheckHitRound();
            }

            if (currentLifeTime >= lifeTime)
            {
                if (particleObject != null)
                {
                    ParticleSystem[] allParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in allParticles)
                    {
                        ps.Stop();
                    }
                }
                if (audioObject != null)
                {
                    AudioSource[] allAudios = audioObject.GetComponentsInChildren<AudioSource>();
                    foreach (AudioSource audio in allAudios)
                    {
                        audio.Stop();
                    }
                }
                ObjectPoolManager.Instance.Despawn(gameObject);
            }

            currentLifeTime += Time.deltaTime;
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        // 공격이 라운드 형태가 아니면 그리지 않음
        if (!isRound) return;

        // 기즈모 색상 설정 (빨간색 투명)
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);

        // 실제 판정 범위 계산과 동일하게 스케일 반영
        float actualRadius = (radius * transform.localScale.x) / 2;
        float actualInnerRadius = (innerRadius * transform.localScale.x) / 2;

        // 부채꼴 그리기
        Handles.color = new Color(1f, 0f, 0f, 0.2f);

        // 부채꼴의 시작 각도 계산 (forwardDir 기준 좌우로 angle/2 만큼)
        Vector3 forward = forwardDir != Vector3.zero ? forwardDir : transform.forward;

        // Handles.DrawSolidArc는 (중심점, 법선벡터, 시작방향, 각도, 반지름) 순서입니다.
        // 법선은 보통 위쪽(Vector3.up)을 사용합니다.
        Handles.DrawSolidArc(centerPos, Vector3.up,
                             Quaternion.Euler(0, -angle / 2, 0) * forward,
                             angle, actualRadius);

        // 내부 반경(InnerRadius) 영역을 파내어 도넛 모양으로 시각화 (선으로 표현)
        if (actualInnerRadius > 0)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireArc(centerPos, Vector3.up,
                                Quaternion.Euler(0, -angle / 2, 0) * forward,
                                angle, actualInnerRadius);
        }
    }
    #endif
}
