using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Arthur/AuraSlash")]
public class AuraSlash : SkillData
{
    public override void OnAnimationEvent(UnitController user, int number)
    {
        ArthurController arthur = user as ArthurController;
        if (arthur == null) return;

        Quaternion rotation = Quaternion.LookRotation(arthur.transform.forward);
        Vector3 spawnPos = arthur.transform.position + arthur.transform.forward + (Vector3.up * 2f);

        float range = 10f + ((arthur.status.skillRange - 1f) * 1f);
        Vector3 size = new Vector3(range, 1f, range);

        ObjectPoolManager.Instance
            .Spawn("ArthurSkill2", spawnPos, rotation)
            .GetComponent<HitController>()
            .Initialize(
                (arthur.status.damage * 2f) * arthur.status.skillDamage,
                40f,
                0.2f,
                0f,
                0.5f,
                0,
                arthur.faction,
                size
            );

        ObjectPoolManager.Instance
            .Spawn("AudioObject", arthur.transform.position, Quaternion.identity)
            .GetComponent<AudioObject>()
            .PlayAudio("Sword/AuraSlash", "Weapon", 1);
    }
}