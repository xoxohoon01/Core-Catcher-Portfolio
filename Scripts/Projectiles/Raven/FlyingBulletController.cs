using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBulletController : BulletController
{
    float rotateSpeed = 16f;

    protected override void CheckHit(UnitController target)
    {
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.status.hp -= damage;
        DamageNumber damageNumber = healthHitPrefab.Spawn(target.transform.position, damage);

        if (SkillManager.Instance.CheckSkillUnlocked("Raven", 8))
        {
            if (CardManager.Instance.artifactEffectLevel["Targeting"] > 0)
            {
                if (Random.Range(0.0f, 1.0f) > 0.8f)
                {
                    ObjectPoolManager.Instance.Spawn("Targeting", target.transform.position, Quaternion.identity).GetComponent<TargetingController>()
                    .Initialize(
                    (PlayerManager.Instance.GetPlayer().status.damage * 0.2f) * CardManager.Instance.artifactEffectLevel["Targeting"],
                    0,
                    3f,
                    0.625f,
                    5f,
                    0.5f,
                    Faction.Player,
                    Vector3.one);
                }
            }
        }
    }

    protected override void Move()
    {
        MonsterController nearest = FindNearestMonster();

        Vector3 nowPosition = transform.position;

        // 1. 목표 방향 계산 (Y 제거 → 수평 유도)
        Vector3 targetDirection;

        if (nearest != null)
        {
            targetDirection = nearest.transform.position - nowPosition;
            targetDirection.y = 0f;                 // ★ Y축 제거
            targetDirection.Normalize();
        }
        else
        {
            targetDirection = transform.forward;
            targetDirection.y = 0f;
            targetDirection.Normalize();
        }

        // 2. 현재 전방 방향도 Y 제거
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        rotateSpeed += 0.2f;

        // 3. 수평 회전만 수행
        Vector3 smoothDirection = Vector3.RotateTowards(
            forward,
            targetDirection,
            rotateSpeed * Time.fixedDeltaTime,
            0f
        );

        // 4. Y축 회전만 적용
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

        // 예: 모든 몬스터를 전역적으로 관리한다면
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
