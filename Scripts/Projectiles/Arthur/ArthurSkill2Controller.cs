using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArthurSkill2Controller : HitController
{
    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);

        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.GetKnockback(transform.forward, 8, 0.25f);
        target.GetAirBorne(4f);
    }
}
