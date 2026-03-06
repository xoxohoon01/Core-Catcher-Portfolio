using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenAttackController : BulletController
{
    static readonly float[] angles1 = { 0f };
    static readonly float[] angles2 = { -45f, 45f };
    static readonly float[] angles3 = { 0f, -45f, 45f };

    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);

        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        int flyingBulletLevel = CardManager.Instance.artifactEffectLevel["FlyingBullet"];

        if (flyingBulletLevel > 0)
        {
            PlayerController player = PlayerManager.Instance.GetPlayer();

            ArtifactCardScriptableObject card = CardManager.Instance.GetArtifact("FlyingBullet");
            float damage = player.status.damage * card.GetValue(AttributeType.amount, flyingBulletLevel);

            Quaternion startRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);

            float[] angles =
                flyingBulletLevel < 3 ? angles1 :
                flyingBulletLevel < 5 ? angles2 :
                angles3;

            foreach (float angle in angles)
            {
                Quaternion rot = startRotation * Quaternion.Euler(0, angle, 0);

                ObjectPoolManager.Instance
                    .Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), rot)
                    .GetComponent<FlyingBulletController>()
                    .Initialize(
                        damage,
                        1,
                        80f,
                        10f,
                        false,
                        sender,
                        Faction.Player,
                        Vector3.one);
            }
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
