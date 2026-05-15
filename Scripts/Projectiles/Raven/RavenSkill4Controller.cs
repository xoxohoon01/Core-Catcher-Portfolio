using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenSkill4Controller : BulletController
{
    protected override void CheckHit(UnitController target)
    {
        // 탄환이 아닌 히트 단위로 크리티컬 재계산
        critResult = CriticalCalculator.Calculate(sender);
        damage = rawDamage * critResult.multiplier;
        damageNumberPrefab = critResult.damageNumber;

        base.CheckHit(target);

        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.GetKnockback(transform.forward, 18f, 0.6f);
    }
}
