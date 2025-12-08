using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenAttackController : BulletController
{
    protected override void CheckHit(UnitController target)
    {
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.status.hp -= damage;
        DamageNumber damageNumber = healthHitPrefab.Spawn(target.transform.position, damage);

        PlayerController player = PlayerManager.Instance.GetPlayer();

        if (CardManager.Instance.artifactEffectLevel["FlyingBullet"] > 0)
        {
            Quaternion startRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
            ObjectPoolManager.Instance.Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), startRotation).GetComponent<FlyingBulletController>().Initialize(player.status.damage, 1, 80f, 10f, false, Faction.Player, Vector3.one);
        }
        if (CardManager.Instance.artifactEffectLevel["Targeting"] > 0)
        {
            ObjectPoolManager.Instance.Spawn("Targeting", target.transform.position, Quaternion.identity).GetComponent<TargetingController>().Initialize(player.status.damage/30, 0, 3f, 0.625f, 5f, 0.5f, Faction.Player, Vector3.one);
        }
    }
}
