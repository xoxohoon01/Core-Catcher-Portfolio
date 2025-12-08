using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenSkill2ShotgunController : HitController
{
    protected override void CheckHit(UnitController target)
    {
        target.status.hp -= damage;
        DamageNumber damageNumber = healthHitPrefab.Spawn(target.transform.position, damage);
        target.GetKnockback(transform.forward, 7, 0.5f);
        target.GetAirBorne(3.5f);
    }
}
