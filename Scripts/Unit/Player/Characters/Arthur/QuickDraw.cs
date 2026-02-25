using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Arthur/QuickDraw")]
public class QuickDraw : SkillData
{
    public override void OnAnimationEvent(UnitController user, int number)
    {
        ArthurController arthur = user as ArthurController;
        if (arthur == null) return;

        Quaternion baseRotation = Quaternion.LookRotation(arthur.transform.forward);
        Quaternion rotation = baseRotation * Quaternion.Euler(0f, 0f, 180f);
        Vector3 spawnPos = arthur.transform.position + arthur.transform.forward + (Vector3.up * 2f);

        float range = 15f + ((arthur.status.skillRange - 1f) * 5f);
        Vector3 size = new Vector3(range, 1f, range);

        ObjectPoolManager.Instance
            .Spawn("ArthurSkill1", spawnPos, rotation)
            .GetComponent<HitController>()
            .Initialize(
                (arthur.status.damage * 2f) * arthur.status.skillDamage,
                0f,
                0.2f,
                0f,
                0.5f,
                0,
                arthur,
                arthur.faction,
                size
            );

        ObjectPoolManager.Instance
            .Spawn("AudioObject", arthur.transform.position, Quaternion.identity)
            .GetComponent<AudioObject>()
            .PlayAudio("Sword/QuickDraw", "Weapon", 1);
    }
}
