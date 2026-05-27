using UnityEngine;

// AfterBurner 아티팩트 전용 탄환 — AfterBurner 카운트 제외, FlyingBullet · Targeting은 OnBulletHit으로 발동
public class AfterBurnerController : BulletController
{
    public override void Initialize(float baseDamage, int critLevel, float startSpeed, float startLifeTime, bool isStartPenetration, UnitController sender, Faction senderFaction, Vector3 size)
    {
        base.Initialize(baseDamage, critLevel, startSpeed, startLifeTime, isStartPenetration, sender, senderFaction, size);
        // AfterBurner 카운트에서 제외 (무한 루프 방지), FlyingBullet · Targeting은 OnBulletHit으로 발동
        countAsAttackHit = false;
    }

    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");
    }
}
