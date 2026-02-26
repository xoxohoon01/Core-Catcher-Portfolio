using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenAttackController : BulletController
{
    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);

        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        PlayerController player = PlayerManager.Instance.GetPlayer();

        int flyingBulletLevel = CardManager.Instance.artifactEffectLevel["FlyingBullet"];
        if (flyingBulletLevel > 0 && flyingBulletLevel < 3)
        {
            Quaternion startRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
            ObjectPoolManager.Instance.Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), startRotation).GetComponent<FlyingBulletController>()
                .Initialize(
                (player.status.damage * 0.15f) + ((player.status.damage * 0.5f) * CardManager.Instance.artifactEffectLevel["FlyingBullet"]),
                1,
                80f,
                10f,
                false,
                sender,
                Faction.Player,
                Vector3.one);
        }
        else if (flyingBulletLevel >= 3 && flyingBulletLevel < 5)
        {
            Quaternion startRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
            ObjectPoolManager.Instance.Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), startRotation * Quaternion.Euler(0, -45, 0)).GetComponent<FlyingBulletController>()
                .Initialize(
                (player.status.damage * 0.15f) + ((player.status.damage * 0.5f) * CardManager.Instance.artifactEffectLevel["FlyingBullet"]),
                1,
                80f,
                10f,
                false,
                sender,
                Faction.Player,
                Vector3.one);

            ObjectPoolManager.Instance.Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), startRotation * Quaternion.Euler(0, 45, 0)).GetComponent<FlyingBulletController>()
                .Initialize(
                (player.status.damage * 0.15f) + ((player.status.damage * 0.5f) * CardManager.Instance.artifactEffectLevel["FlyingBullet"]),
                1,
                80f,
                10f,
                false,
                sender,
                Faction.Player,
                Vector3.one);
        }
        else if (flyingBulletLevel == 5)
        {
            Quaternion startRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
            ObjectPoolManager.Instance.Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), startRotation).GetComponent<FlyingBulletController>()
                .Initialize(
                (player.status.damage * 0.15f) + ((player.status.damage * 0.5f) * CardManager.Instance.artifactEffectLevel["FlyingBullet"]),
                1,
                80f,
                10f,
                false,
                sender,
                Faction.Player,
                Vector3.one);

            ObjectPoolManager.Instance.Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), startRotation * Quaternion.Euler(0, -45, 0)).GetComponent<FlyingBulletController>()
                .Initialize(
                (player.status.damage * 0.15f) + ((player.status.damage * 0.5f) * CardManager.Instance.artifactEffectLevel["FlyingBullet"]),
                1,
                80f,
                10f,
                false,
                sender,
                Faction.Player,
                Vector3.one);

            ObjectPoolManager.Instance.Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), startRotation * Quaternion.Euler(0, 45, 0)).GetComponent<FlyingBulletController>()
                .Initialize(
                (player.status.damage * 0.15f) + ((player.status.damage * 0.5f) * CardManager.Instance.artifactEffectLevel["FlyingBullet"]),
                1,
                80f,
                10f,
                false,
                sender,
                Faction.Player,
                Vector3.one);
        }

        if (CardManager.Instance.artifactEffectLevel["Targeting"] > 0)
        {
            if (Random.Range(0.0f, 1.0f) > 0.8f)
            {
                int level = CardManager.Instance.artifactEffectLevel["Targeting"];
                ObjectPoolManager.Instance.Spawn("Targeting", target.transform.position, Quaternion.identity).GetComponent<TargetingController>()
                .Initialize(
                (level == 5) ? (30) : (5 * level),
                0,
                3f,
                0.625f,
                5f,
                0.5f,
                sender,
                Faction.Player,
                Vector3.one);
            }
        }
    }
}
