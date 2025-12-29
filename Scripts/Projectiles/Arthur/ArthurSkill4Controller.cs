using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArthurSkill4Controller : HitController
{
    protected override void CheckHit(UnitController target)
    {
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.status.hp -= damage;
        DamageNumber damageNumber = healthHitPrefab.Spawn(target.transform.position, damage);
        target.GetKnockback(transform.forward, 4, 0.25f);
        target.GetAirBorne(4f);
    }
}
