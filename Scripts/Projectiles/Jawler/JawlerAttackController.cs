using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawlerAttackController : BulletController
{
    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");
    }
}
