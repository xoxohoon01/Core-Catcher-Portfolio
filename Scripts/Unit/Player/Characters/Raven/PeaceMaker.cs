using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Raven/PeaceMaker")]
public class PeaceMaker : SkillData
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

        Vector3 finalSize =
            SkillManager.Instance.Skill["Raven"][6].isUnlocked ? skillSize : originalSize;

        ObjectPoolManager.Instance
            .Spawn("RavenSkill1", spawnPos, rotation)
            .GetComponent<HitController>()
            .Initialize(
                (raven.status.damage * 2f) * raven.status.skillDamage,
                0f,
                0.2f,
                0f,
                0.5f,
                0,
                raven,
                raven.faction,
                finalSize
            );

        ObjectPoolManager.Instance
            .Spawn("AudioObject", raven.transform.position, Quaternion.identity)
            .GetComponent<AudioObject>()
            .PlayAudio("Shotgun", "Weapon", 1);
    }
}