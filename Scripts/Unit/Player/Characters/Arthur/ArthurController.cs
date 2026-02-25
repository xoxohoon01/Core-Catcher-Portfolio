using System.Linq;
using UnityEngine;

public class ArthurController : PlayerController
{
    public bool isSkill3;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Initialize()
    {
        base.Initialize();

        status.damage = characterData.damage +
            (SkillManager.Instance.Skill["Arthur"][0].isUnlocked ? characterData.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][1].isUnlocked ? characterData.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][2].isUnlocked ? characterData.damage * 0.1f : 0)
            ;

        status.maxHP = characterData.maxHP +
            (SkillManager.Instance.Skill["Arthur"][3].isUnlocked ? characterData.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][4].isUnlocked ? characterData.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][5].isUnlocked ? characterData.maxHP * 0.1f : 0);

        status.hp = status.maxHP;
    }

    protected override void DashInitialize()
    {

    }

    public override void BasicAttackInitialize(int number)
    {
        base.BasicAttackInitialize(number);

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        if (number == 0)
        {
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 0)).GetComponent<HitController>().Initialize(status.damage, 0, 0.2f, 0, 0.5f, 0, this, faction, new Vector3(10, 3, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Sword/Slash/ArthurAttack1", "Weapon", 1);
        }
        else if (number == 1)
        {
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, -175)).GetComponent<HitController>().Initialize(status.damage * 1.3f, 0, 0.2f, 0, 0.5f, 0, this, faction, new Vector3(10, 5, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Sword/Slash/ArthurAttack2", "Weapon", 1);
        }
        else if (number == 2)
        {
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, -35)).GetComponent<HitController>().Initialize(status.damage * 2.5f, 0, 0.2f, 0, 0.5f, 0, this, faction, new Vector3(10, 5, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Sword/Slash/ArthurAttack3", "Weapon", 1);
        }
        else if (number == 3)
        {
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 180)).GetComponent<HitController>().Initialize(status.damage * 2.5f, 0, 0.2f, 0, 0.5f, 0, this, faction, new Vector3(10, 3, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Sword/Slash/ArthurAttack4", "Weapon", 1);
        }
    }
}
