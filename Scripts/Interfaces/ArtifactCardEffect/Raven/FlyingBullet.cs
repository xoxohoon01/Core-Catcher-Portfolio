using UnityEngine;

// 기본공격 · AfterBurner 명중 시 플레이어 전방으로 유도탄 발사
// OnBulletHit 이벤트 구독 — triggersBulletHit=false인 탄환(FlyingBullet 자신)은 발동하지 않아 루프 없음
public class FlyingBullet : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private System.Action<UnitController, Vector3> hitCallback;

    static readonly float[] angles1 = { 0f };
    static readonly float[] angles2 = { -45f, 45f };
    static readonly float[] angles3 = { 0f, -45f, 45f };

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

        float damage = player.status.damage * card.GetValue(AttributeType.amount, level);
        Quaternion startRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);

        float[] angles =
            level < 3 ? angles1 :
            level < 5 ? angles2 :
            angles3;

        foreach (float angle in angles)
        {
            Quaternion rot = startRotation * Quaternion.Euler(0, angle, 0);

            ObjectPoolManager.Instance
                .Spawn("FlyingBullet", player.transform.position + (Vector3.up * 2), rot)
                .GetComponent<FlyingBulletController>()
                .Initialize(damage, 1, 80f, 10f, false, player, Faction.Player, Vector3.one);
        }
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
