using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArthurSkill3Controller : HitController
{
    private Transform arthur;

    public override void Initialize(float damage, float moveSpeed, float hitTime, float startDelay, float lifeTime, float multiHitDelay, Faction senderFaction, Vector3 size)
    {
        base.Initialize(damage, moveSpeed, hitTime, startDelay, lifeTime, multiHitDelay, senderFaction, size);

        arthur = PlayerManager.Instance.GetPlayer().transform;
    }

    protected override void Update()
    {
        base.Update();

        transform.position = arthur.position + Vector3.up;
        transform.rotation = arthur.rotation;
    }

    protected override void CheckHit(UnitController target)
    {
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.status.hp -= damage;
        DamageNumber damageNumber = healthHitPrefab.Spawn(target.transform.position, damage);
        //target.GetKnockback(transform.forward, 8, 0.25f);
        target.GetAirBorne(8f);
    }
}
