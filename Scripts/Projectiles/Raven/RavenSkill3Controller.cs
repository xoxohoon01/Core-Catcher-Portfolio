using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenSkill3Controller : BulletController
{
    protected override void CheckHit(UnitController target)
    {
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.status.hp -= damage;

        bool isSkilled = SkillManager.Instance.Skill["Raven"][7].isUnlocked;
        target.GetKnockback(transform.forward, isSkilled ? 12f : 6f, 0.6f);
        if (!isSkilled)
            target.GetAirBorne(6.0f);
    }
}
