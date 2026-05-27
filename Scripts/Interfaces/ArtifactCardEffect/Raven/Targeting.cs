using UnityEngine;

// 기본공격 · AfterBurner 명중 시 확률로 해당 위치에 낙뢰 히트 발동
// OnBulletHit 이벤트 구독 — FlyingBullet→Targeting 연계는 FlyingBulletController에서 별도 처리
public class Targeting : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private System.Action<UnitController, Vector3> hitCallback;

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        hitCallback = OnBulletHit;
        player.OnBulletHit += hitCallback;
    }

    private void OnBulletHit(UnitController target, Vector3 hitPos)
    {
        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        if (level <= 0) return;

        if (Random.Range(0.0f, 1.0f) > card.GetValue(AttributeType.chance, level)) return;

        ObjectPoolManager.Instance
            .Spawn("Targeting", target.transform.position, Quaternion.identity)
            .GetComponent<TargetingController>()
            .Initialize(
                player.status.damage * card.GetValue(AttributeType.amount, level),
                0, 3f, 0.625f, 5f, 0.5f,
                player, Faction.Player, Vector3.one);
    }

    public void Update() { }

    public void Remove()
    {
        if (hitCallback != null)
        {
            player.OnBulletHit -= hitCallback;
            hitCallback = null;
        }
    }
}
