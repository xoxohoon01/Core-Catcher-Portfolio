using System.Linq;
using UnityEngine;

public class ArthurController : PlayerController
{
    public float barrierDelay;
    public bool isSkill3;

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
            (SkillManager.Instance.Skill["Arthur"][0].isUnlocked ? character.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][1].isUnlocked ? character.damage * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][2].isUnlocked ? character.damage * 0.1f : 0)
            ;

        status.maxHP = character.maxHP +
            (SkillManager.Instance.Skill["Arthur"][3].isUnlocked ? character.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][4].isUnlocked ? character.maxHP * 0.1f : 0) +
            (SkillManager.Instance.Skill["Arthur"][5].isUnlocked ? character.maxHP * 0.1f : 0);

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
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 0)).GetComponent<HitController>().Initialize(status.damage, 0, 0.2f, 0, 0.5f, 0, faction, new Vector3(10, 3, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 1)
        {
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, -145)).GetComponent<HitController>().Initialize(status.damage * 1.3f, 0, 0.2f, 0, 0.5f, 0, faction, new Vector3(10, 5, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 2)
        {
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 35)).GetComponent<HitController>().Initialize(status.damage * 2.5f, 0, 0.2f, 0, 0.5f, 0, faction, new Vector3(10, 5, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 3)
        {
            ObjectPoolManager.Instance.Spawn("ArthurAttack", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 180)).GetComponent<HitController>().Initialize(status.damage * 2.5f, 0, 0.2f, 0, 0.5f, 0, faction, new Vector3(10, 3, 10));
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
    }

    public override void Skill1Initialize(int number)
    {
        base.Skill1Initialize(number);

        moveVector = Vector3.zero;

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        Vector3 originalSize = new Vector3(15 + ((status.skillRange - 1) * 5.0f), 1, 15 + ((status.skillRange - 1) * 5.0f));
        ObjectPoolManager.Instance.Spawn("ArthurSkill1", transform.position + (transform.forward) + (Vector3.up * 2), rotation)
            .GetComponent<HitController>()
            .Initialize(status.damage * 2f, 0f, 0.2f, 0f, 0.5f, 0, faction, originalSize);

        ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Shotgun", "Weapon", 1);
    }
    public override void Skill2Initialize(int number)
    {
        base.Skill2Initialize(number);

        moveVector = Vector3.zero;

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        Vector3 originalSize = new Vector3(8 + ((status.skillRange - 1) * 1.0f), 1, 8 + ((status.skillRange - 1) * 1.0f));
        ObjectPoolManager.Instance.Spawn("ArthurSkill1", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, -45))
            .GetComponent<HitController>()
            .Initialize(status.damage * 2f, 40f, 0.2f, 0f, 0.5f, 0, faction, originalSize);

        ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
    }

    public override void Skill3Initialize(int number)
    {
        base.Skill3Initialize(number);

        moveVector = Vector3.zero;

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        Vector3 originalSize = new Vector3(8 + ((status.skillRange - 1) * 1.0f), 8 + ((status.skillRange - 1) * 0.5f), 8 + ((status.skillRange - 1) * 1.0f));
        var clip = animator.runtimeAnimatorController
                   .animationClips
                   .FirstOrDefault(c => c.name == character.skill3ClipName);
        float totalTime =
            (character.skill3Span /
            (1 + ((status.attackSpeed / character.attackSpeed) * 0.1f)))
            / status.skillSpeed;

        float activeTime = totalTime * 0.5f;
        ObjectPoolManager.Instance.Spawn("ArthurSkill3", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 0))
            .GetComponent<HitController>()
            .Initialize(status.damage * 2f, 0f, 0.5f, 0f, activeTime, 0, faction, originalSize);

        ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("RisingShot", "Weapon", 1);
        dashDelay = 1;
    }

    public override void Skill4Initialize(int number)
    {
        base.Skill4Initialize(number);

        moveVector = Vector3.zero;

        Quaternion lookRotation = Quaternion.LookRotation(targetVector);
        Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

        if (number == 0)
        {
            Vector3 originalSize = new Vector3(10 + ((status.skillRange - 1) * 2.0f), 3, 10 + ((status.skillRange - 1) * 2.0f));
            ObjectPoolManager.Instance.Spawn("ArthurSkill4", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 0)).GetComponent<HitController>().Initialize(status.damage * 1.2f, 0, 0.2f, 0, 0.5f, 0, faction, originalSize);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 1)
        {
            Vector3 originalSize = new Vector3(10 + ((status.skillRange - 1) * 2.0f), 5, 10 + ((status.skillRange - 1) * 2.0f));
            ObjectPoolManager.Instance.Spawn("ArthurSkill4", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, -145)).GetComponent<HitController>().Initialize(status.damage * 1.2f, 0, 0.2f, 0, 0.5f, 0, faction, originalSize);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 2)
        {
            Vector3 originalSize = new Vector3(10 + ((status.skillRange - 1) * 2.0f), 5, 10 + ((status.skillRange - 1) * 2.0f));
            ObjectPoolManager.Instance.Spawn("ArthurSkill4", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 30)).GetComponent<HitController>().Initialize(status.damage * 1.2f, 0, 0.2f, 0, 0.5f, 0, faction, originalSize);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
        else if (number == 3)
        {
            Vector3 originalSize = new Vector3(10 + ((status.skillRange - 1) * 2.0f), 3, 10 + ((status.skillRange - 1) * 2.0f));
            ObjectPoolManager.Instance.Spawn("ArthurSkill4", transform.position + (transform.forward) + (Vector3.up * 2), Quaternion.Euler(0, rotation.eulerAngles.y, 180)).GetComponent<HitController>().Initialize(status.damage * 1.2f, 0, 0.2f, 0, 0.5f, 0, faction, originalSize);
            ObjectPoolManager.Instance.Spawn("AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("Pistol", "Weapon", 1);
        }
    }
}
