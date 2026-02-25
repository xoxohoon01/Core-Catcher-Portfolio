using UnityEngine;

public class ArthurSkill3Controller : HitController
{
    private Transform owner;

    public void SetOwner(Transform ownerTransform)
    {
        owner = ownerTransform;
    }

    public override void Initialize(
        float baseDamage,
        float moveSpeed,
        float hitTime,
        float startDelay,
        float lifeTime,
        float multiHitDelay,
        UnitController sender,
        Faction senderFaction,
        Vector3 size)
    {
        base.Initialize(baseDamage, moveSpeed, hitTime, startDelay, lifeTime, multiHitDelay, sender, senderFaction, size);
    }

    protected override void Update()
    {
        base.Update();

        if (owner == null) return;

        transform.position = owner.position + Vector3.up;
        transform.rotation = owner.rotation;
    }

    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);

        ObjectPoolManager.Instance
            .Spawn("AudioObject", transform.position, Quaternion.identity)
            .GetComponent<AudioObject>()
            .PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.GetAirBorne(8f);
    }
}