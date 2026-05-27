using UnityEngine;

// Raven 기본공격 전용 탄환 — AfterBurner 카운트 및 FlyingBullet · Targeting 발동 활성화
public class RavenAttackController : BulletController
{
    public override void Initialize(float baseDamage, int critLevel, float startSpeed, float startLifeTime, bool isStartPenetration, UnitController sender, Faction senderFaction, Vector3 size)
    {
        base.Initialize(baseDamage, critLevel, startSpeed, startLifeTime, isStartPenetration, sender, senderFaction, size);
        countAsAttackHit = true;  // 기본공격만 AfterBurner 카운트
        triggersBulletHit = true; // 기본공격만 FlyingBullet · Targeting 발동
    }

    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);
        // FlyingBullet · Targeting 발동은 OnBulletHit 이벤트(FlyingBullet.cs, Targeting.cs)가 처리
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");
    }
}
