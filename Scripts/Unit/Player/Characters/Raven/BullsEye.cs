using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Raven/BullsEye")]
public class BullsEye : SkillData
{
    public override void OnAnimationEvent(UnitController user, int number)
    {
        RavenController raven = user as RavenController;
        if (raven == null) return;

        Quaternion rotation = Quaternion.LookRotation(raven.transform.forward);
        Vector3 spawnPos = raven.transform.position + raven.transform.forward + (Vector3.up * 2);

        ObjectPoolManager.Instance
            .Spawn("RavenSkill4", spawnPos, rotation)
            .GetComponent<BulletController>()
            .Initialize(
                (raven.status.damage * 3.5f) * raven.status.skillDamage,
                raven.CheckCritical(),
                150f,
                2f,
                true,
                raven,
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
            .PlayAudio("Sniper", "Weapon", 1);
    }
}