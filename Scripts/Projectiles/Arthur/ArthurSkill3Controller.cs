using UnityEngine;

public class ArthurSkill3Controller : HitController
{
    private Transform owner;

    public void SetOwner(Transform ownerTransform)
    {
        owner = ownerTransform;
    }

    public override void Initialize(
        float damage,
        float moveSpeed,
        float hitTime,
        float startDelay,
        float lifeTime,
        float multiHitDelay,
        Faction senderFaction,
        Vector3 size)
    {
        base.Initialize(damage, moveSpeed, hitTime, startDelay, lifeTime, multiHitDelay, senderFaction, size);
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
        ObjectPoolManager.Instance
            .Spawn("AudioObject", transform.position, Quaternion.identity)
            .GetComponent<AudioObject>()
            .PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        target.status.hp -= damage;
        healthHitPrefab.Spawn(target.transform.position, damage);

        target.GetAirBorne(8f);
    }
}