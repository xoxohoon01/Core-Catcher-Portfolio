using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenSkill3Controller : BulletController
{
    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);

        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        bool isSkilled = SkillManager.Instance.Skill["Raven"][7].isUnlocked;
        if (!isSkilled)
        {
            target.GetAirBorne(9.0f);
            target.GetKnockback(transform.forward, 6f, 0.6f);
        }
        else
        {
            target.GetKnockback(transform.forward, 15f, 0.6f);
        }
    }
}
