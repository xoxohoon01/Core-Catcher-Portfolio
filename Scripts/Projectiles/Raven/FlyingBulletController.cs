using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBulletController : BulletController
{
    float rotateSpeed = 16f;

    public override void Initialize(float baseDamage, int critLevel, float startSpeed, float startLifeTime, bool isStartPenetration, UnitController sender, Faction senderFaction, Vector3 size)
    {
        base.Initialize(baseDamage, critLevel, startSpeed, startLifeTime, isStartPenetration, sender, senderFaction, size);
        // FlyingBullet 명중 시 AfterBurner 카운트는 포함, FlyingBullet 재발동만 제외 (루프 방지)
        countAsAttackHit = true;
        triggersBulletHit = false;
    }

    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        // Targeting 아티팩트 보유 시 FlyingBullet → Targeting 연계
        int level = CardManager.Instance.artifactEffectLevel["Targeting"];
        if (level > 0)
        {
            ArtifactCardScriptableObject card = CardManager.Instance.GetArtifact("Targeting");
            if (Random.Range(0.0f, 1.0f) <= card.GetValue(AttributeType.chance, level))
            {
                ObjectPoolManager.Instance.Spawn("Targeting", target.transform.position, Quaternion.identity)
                    .GetComponent<TargetingController>()
                    .Initialize(
                        PlayerManager.Instance.GetPlayer().status.damage * card.GetValue(AttributeType.amount, level),
                        0, 3f, 0.625f, 5f, 0.5f,
                        sender, Faction.Player, Vector3.one);
            }
        }
    }

    protected override void Move()
    {
        MonsterController nearest = FindNearestMonster();

        Vector3 nowPosition = transform.position;

        // 1. 목표 방향 계산 (Y 값은 제외한 수평 방향)
        Vector3 targetDirection;

        if (nearest != null)
        {
            targetDirection = nearest.transform.position - nowPosition;
            targetDirection.y = 0f;                 // Y축 제외
            targetDirection.Normalize();
        }
        else
        {
            targetDirection = transform.forward;
            targetDirection.y = 0f;
            targetDirection.Normalize();
        }

        // 2. 현재 진행 방향도 Y 제외
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        rotateSpeed += 0.2f;

        // 3. 부드럽게 회전할 방향 계산
        Vector3 smoothDirection = Vector3.RotateTowards(
            forward,
            targetDirection,
            rotateSpeed * Time.fixedDeltaTime,
            0f
        );

        // 4. Y축 회전은 고정
        transform.rotation = Quaternion.LookRotation(smoothDirection);

        // 5. 이동 (Y 고정)
        Vector3 newPos = transform.position + smoothDirection * (moveSpeed * Time.fixedDeltaTime);
        newPos.y = 2f;

        HandleMove(newPos);
    }


    private MonsterController FindNearestMonster()
    {
        float minDist = float.MaxValue;
        MonsterController nearest = null;

        Collider[] monsters = Physics.OverlapSphere(transform.position, 100, LayerMask.GetMask("Monster"), QueryTriggerInteraction.Ignore);

        // 모든 몬스터를 순회하며 가장 가까운 대상을 찾는다
        foreach (var monster in monsters)
        {
            if (monster == null || monster.GetComponent<MonsterController>().isDead)
                continue;

            float dist = Vector3.Distance(transform.position, monster.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = monster.GetComponent<MonsterController>();
            }
        }

        return nearest;
    }
}
