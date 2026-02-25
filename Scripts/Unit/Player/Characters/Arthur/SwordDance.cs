using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Arthur/SwordDance")]
public class SwordDance : SkillData
{
    public override void OnAnimationEvent(UnitController user, int number)
    {
        ArthurController arthur = user as ArthurController;
        if (arthur == null) return;

        Quaternion baseRotation = Quaternion.LookRotation(arthur.transform.forward);
        Quaternion rotation;
        if (number == 0)
            rotation = baseRotation * Quaternion.Euler(0f, 0f, -25f);
        else
            rotation = baseRotation * Quaternion.Euler(0f, 0f, -155f);
        Vector3 spawnPos = arthur.transform.position + arthur.transform.forward + (Vector3.up * 2f);

        float range = 10f + ((arthur.status.skillRange - 1f) * 2f);
        Vector3 size = new Vector3(range, 3f, range);

        ObjectPoolManager.Instance
            .Spawn("ArthurSkill4", spawnPos, rotation)
            .GetComponent<HitController>()
            .Initialize(
                (arthur.status.damage * 1.2f) * arthur.status.skillDamage,
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
            .PlayAudio("Sword/Slash/ArthurAttack1", "Weapon", 1);
    }
}