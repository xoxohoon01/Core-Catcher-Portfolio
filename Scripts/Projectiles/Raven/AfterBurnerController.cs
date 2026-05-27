using UnityEngine;

// AfterBurner 아티팩트 전용 탄환 — AfterBurner 카운트 제외, Targeting만 발동 (FlyingBullet 제외)
public class AfterBurnerController : BulletController
{
    public override void Initialize(float baseDamage, int critLevel, float startSpeed, float startLifeTime, bool isStartPenetration, UnitController sender, Faction senderFaction, Vector3 size)
    {
        base.Initialize(baseDamage, critLevel, startSpeed, startLifeTime, isStartPenetration, sender, senderFaction, size);
        // AfterBurner 루프 방지, FlyingBullet 발동 제외
        countAsAttackHit = false;
        triggersBulletHit = false;
    }

    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");

        // Targeting 아티팩트 보유 시 발동
        int level = CardManager.Instance.artifactEffectLevel["Targeting"];
        if (level > 0)
        {
            ArtifactCardScriptableObject card = CardManager.Instance.GetArtifact("Targeting");
            if (Random.Range(0.0f, 1.0f) <= card.GetValue(AttributeType.chance, level))
            {
                ObjectPoolManager.Instance.Spawn("Targeting", target.transform.position, Quaternion.identity)
                    .GetComponent<TargetingController>()
                    .Initialize(
                        PlayerManager.Instance.GetPlayer().status.damage * card.GetValue(AttributeType.amount, level),
                        0, 3f, 0.625f, 5f, 0.5f,
                        sender, Faction.Player, Vector3.one);
            }
        }
    }
}
