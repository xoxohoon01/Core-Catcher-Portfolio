using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Raven/WeaponCombination")]
public class WeaponCombination : SkillData
{
    public override void OnAnimationEvent(UnitController user, int number)
    {
        RavenController raven = user as RavenController;
        if (raven == null) return;

        Quaternion rotation = Quaternion.LookRotation(raven.transform.forward);
        Vector3 spawnPos = raven.transform.position + raven.transform.forward + (Vector3.up * 2);

        Vector3 originalSize = new Vector3(
            1 + ((raven.status.skillRange - 1) * 0.5f),
            1,
            raven.status.skillRange
        );

        Vector3 skillSize = new Vector3(
            1 + ((raven.status.skillRange - 1) * 1.25f),
            raven.status.skillRange,
            1 + ((raven.status.skillRange - 1) * 0.5f)
        );

        if (number == 0)
        {
            ObjectPoolManager.Instance
                .Spawn("RavenSkill2Bullet", spawnPos, rotation)
                .GetComponent<BulletController>()
                .Initialize(
                    raven.status.damage * 2.3f,
                    raven.CheckCritical(),
                    100f,
                    1f,
                    false,
                    raven.faction,
                    new Vector3(
                        raven.status.skillRange,
                        raven.status.skillRange,
                        raven.status.skillRange
                    )
                );

            ObjectPoolManager.Instance
                .Spawn("AudioObject", raven.transform.position, Quaternion.identity)
                .GetComponent<AudioObject>()
                .PlayAudio("Pistol", "Weapon", 1);
        }
        else
        {
            Vector3 finalSize =
                SkillManager.Instance.Skill["Raven"][6].isUnlocked ? skillSize : originalSize;

            ObjectPoolManager.Instance
                .Spawn("RavenSkill2Shotgun", spawnPos, rotation)
                .GetComponent<HitController>()
                .Initialize(
                    (raven.status.damage * 2f) * raven.status.skillDamage,
                    0f,
                    0.2f,
                    0f,
                    0.5f,
                    0,
                    raven.faction,
                    finalSize
                );

            ObjectPoolManager.Instance
                .Spawn("AudioObject", raven.transform.position, Quaternion.identity)
                .GetComponent<AudioObject>()
                .PlayAudio("Shotgun", "Weapon", 1);
        }
    }
}