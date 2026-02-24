using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Raven/PierceShot")]
public class PierceShot : SkillData
{
    public override void OnAnimationEvent(UnitController user)
    {
        RavenController raven = user as RavenController;
        if (raven == null) return;

        Quaternion rotation = Quaternion.LookRotation(raven.transform.forward);
        Vector3 spawnPos = raven.transform.position + raven.transform.forward + (Vector3.up * 2);

        ObjectPoolManager.Instance
            .Spawn("RavenSkill3", spawnPos, rotation)
            .GetComponent<BulletController>()
            .Initialize(
                (raven.status.damage * 2.75f) * raven.status.skillDamage,
                raven.CheckCritical(),
                100f,
                2f,
                true,
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
            .PlayAudio("RisingShot", "Weapon", 1);
    }
}