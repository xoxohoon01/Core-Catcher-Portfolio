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
    }

    protected override void Move()
    {
        MonsterController nearest = FindNearestMonster();

        // 1. 목표 방향 계산
        Vector3 targetDirection;
        Vector3 nowPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (nearest != null)
        {
            targetDirection = (nearest.transform.position - nowPosition).normalized;
        }
        else
        {
            targetDirection = transform.forward;
        }

        // 2. 부드러운 회전 (RotateTowards)
        rotateSpeed += 0.1f;  // 회전 속도 (필요에 따라 조절)

        Vector3 smoothDirection = Vector3.RotateTowards(
            transform.forward,
            targetDirection,
            rotateSpeed * Time.fixedDeltaTime,
            0f
        );

        // 실제 회전 적용
        transform.rotation = Quaternion.LookRotation(smoothDirection, Vector3.up);

        // 3. "다음 위치" 계산
        Vector3 newPos = transform.position + smoothDirection * (moveSpeed * Time.fixedDeltaTime);
        newPos.y = 2;

        // 4. HandleMove는 절대좌표를 넣어야 한다
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
