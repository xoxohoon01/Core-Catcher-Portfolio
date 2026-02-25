using UnityEngine;

public class RavenController : PlayerController
{
    public GameObject DroneObject;

    public float dodgeSpan;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Initialize()
    {
        base.Initialize();

        status.damage = characterData.damage +
            (SkillManager.Instance.Skill["Raven"][0].isUnlocked ? characterData.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][1].isUnlocked ? characterData.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][2].isUnlocked ? characterData.damage * 0.1f : 0)
            ;

        status.maxHP = characterData.maxHP +
            (SkillManager.Instance.Skill["Raven"][3].isUnlocked ? characterData.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][4].isUnlocked ? characterData.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][5].isUnlocked ? characterData.maxHP * 0.1f : 0);

        status.hp = status.maxHP;
    }

    protected override void DashInitialize()
    {
        base.DashInitialize();

        if (CardManager.Instance.artifactEffectLevel["TacticalDodge"] > 0)
        {
            StatModifier tacticalDodge = new StatModifier
            {
                statType = StatType.CritChance,   // 또는 CritDamage
                type = ModifierType.Add,          // % 기반이면 Mul
                value = 0.1f,                     // +30% 크리 확률
                duration = 3f                     // 3초 지속
            };
            
            AddModifier(tacticalDodge);
        }
    }

    public override void BasicAttackInitialize(int number)
    {
        base.BasicAttackInitialize(number);

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        if (number == 0)
        {
            ObjectPoolManager.Instance.Spawn("RavenAttack", transform.position + (transform.forward) + (Vector3.up * 2), rotation).GetComponent<BulletController>().Initialize(status.damage, CheckCritical(), 100f, 1f, false, this, faction, Vector3.one);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 1)
        {
            ObjectPoolManager.Instance.Spawn("RavenAttack", transform.position + (transform.forward) + (Vector3.up * 2), rotation).GetComponent<BulletController>().Initialize(status.damage * 1.3f, CheckCritical(), 100f, 1f, false, this, faction, Vector3.one);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 2)
        {
            ObjectPoolManager.Instance.Spawn("RavenAttack", transform.position + (transform.forward) + (Vector3.up * 2), rotation).GetComponent<BulletController>().Initialize(status.damage * 2.5f, CheckCritical(), 100f, 1f, false,this, faction, Vector3.one);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
    }
}
