using UnityEngine;

public class ArthurController : PlayerController
{
    public float barrierDelay;

    protected override void Update()
    {
        base.Update();

        if (CardManager.Instance.artifactEffectLevel["Barrier"] > 0)
        {
            barrierDelay = Mathf.Max(barrierDelay - Time.deltaTime, 0);

            if (barrierDelay <= 0)
            {
                status.shield = 30;
                barrierDelay = 20;
            }
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        status.damage = character.damage +
            (SkillManager.Instance.Skill["Raven"][0].isUnlocked ? character.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][1].isUnlocked ? character.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][2].isUnlocked ? character.damage * 0.1f : 0)
            ;

        status.maxHP = character.maxHP +
            (SkillManager.Instance.Skill["Raven"][3].isUnlocked ? character.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][4].isUnlocked ? character.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Raven"][5].isUnlocked ? character.maxHP * 0.1f : 0);

        status.hp = status.maxHP;
    }

    protected override void DashInitialize()
    {
        if (CardManager.Instance.artifactEffectLevel["DecoyBomb"] > 0)
        {
            ObjectPoolManager.Instance.Spawn("DecoyBomb", transform.position, Quaternion.identity).GetComponent<DecoyBombController>()
                .Initialize(status.damage, 0, 1f, 0.5f, 1.5f, 0, Faction.Player, Vector3.one * 5f);
        }
    }

    public override void BasicAttackInitialize(int number)
    {
        base.BasicAttackInitialize(number);

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        if (number == 0)
        {
            ObjectPoolManager.Instance.Spawn("RavenAttack", transform.position + (transform.forward) + (Vector3.up * 2), rotation).GetComponent<BulletController>().Initialize(status.damage, CheckCritical(), 100f, 1f, false, faction, Vector3.one);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 1)
        {
            ObjectPoolManager.Instance.Spawn("RavenAttack", transform.position + (transform.forward) + (Vector3.up * 2), rotation).GetComponent<BulletController>().Initialize(status.damage * 1.3f, CheckCritical(), 100f, 1f, false, faction, Vector3.one);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 2)
        {
            ObjectPoolManager.Instance.Spawn("RavenAttack", transform.position + (transform.forward) + (Vector3.up * 2), rotation).GetComponent<BulletController>().Initialize(status.damage * 2.5f, CheckCritical(), 100f, 1f, false, faction, Vector3.one);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
    }

    public override void Skill1Initialize(int number)
    {
        base.Skill1Initialize(number);

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        Vector3 originalSize = new Vector3(1 + ((status.skillRange - 1) * 0.5f), 1, status.skillRange);
        Vector3 skillSize = new Vector3(1 + ((status.skillRange - 1) * 1.25f), status.skillRange, 1 + ((status.skillRange - 1) * 0.5f));
        ObjectPoolManager.Instance.Spawn("RavenSkill1", transform.position + (transform.forward) + (Vector3.up * 2), rotation)
            .GetComponent<HitController>()
            .Initialize(status.damage * 2f, 0f, 0.2f, 0f, 0.5f, 0, faction, SkillManager.Instance.Skill["Raven"][6].isUnlocked ? skillSize : originalSize);

        ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Shotgun", "Weapon", 1);
    }
    public override void Skill2Initialize(int number)
    {
        base.Skill2Initialize(number);

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        Vector3 originalSize = new Vector3(1 + ((status.skillRange - 1) * 0.5f), 1, status.skillRange);
        Vector3 skillSize = new Vector3(1 + ((status.skillRange - 1) * 1.25f), status.skillRange, 1 + ((status.skillRange - 1) * 0.5f));
        if (number == 0)
        {
            ObjectPoolManager.Instance.Spawn("RavenSkill2Bullet", transform.position + (transform.forward) + (Vector3.up * 2), rotation)
                .GetComponent<BulletController>()
                .Initialize(status.damage * 2.3f, CheckCritical(), 100f, 1f, false, faction, new Vector3(status.skillRange, status.skillRange, status.skillRange));

            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }    
        else if (number == 1)
        {
            ObjectPoolManager.Instance.Spawn("RavenSkill2Shotgun", transform.position + (transform.forward) + (Vector3.up * 2), rotation)
                .GetComponent<HitController>()
                .Initialize(status.damage * 2f, 0f, 0.2f, 0f, 0.5f, 0, faction, SkillManager.Instance.Skill["Raven"][6].isUnlocked ? skillSize : originalSize);

            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Shotgun", "Weapon", 1);
        }
    }

    public override void Skill3Initialize(int number)
    {
        base.Skill3Initialize(number);

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        ObjectPoolManager.Instance.Spawn("RavenSkill3", transform.position + (transform.forward) + (Vector3.up * 2), rotation)
            .GetComponent<BulletController>()
            .Initialize(status.damage * 2.75f, CheckCritical(), 100f, 2f, true, faction, new Vector3(status.skillRange, status.skillRange, status.skillRange));

        ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("RisingShot", "Weapon", 1);
    }
    public override void Skill4Initialize(int number)
    {
        base.Skill4Initialize(number);

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        ObjectPoolManager.Instance.Spawn("RavenSkill4", transform.position + (transform.forward) + (Vector3.up * 2), rotation)
            .GetComponent<BulletController>()
            .Initialize(status.damage * 3.5f, CheckCritical(), 150f, 2f, true, faction, new Vector3(status.skillRange, status.skillRange, status.skillRange));

        ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Sniper", "Weapon", 1);
    }
}
