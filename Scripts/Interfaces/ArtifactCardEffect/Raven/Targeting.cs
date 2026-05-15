using UnityEngine;

public class Targeting : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private System.Action<UnitController, Vector3> hitCallback;

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        hitCallback = OnAttackHit;
        player.OnAttackHit += hitCallback;
    }

    private void OnAttackHit(UnitController hitTarget, Vector3 hitPosition)
    {
        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        if (level <= 0) return;

        // 명중 위치에 범위공격 생성
        float radius = card.GetValue(AttributeType.amount, level);
        float damage = player.status.damage * card.GetValue(AttributeType.duration, level);

        Quaternion rotation = Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f);
        Vector3 forward = rotation * Vector3.forward;

        ObjectPoolManager.Instance
            .Spawn("RavenSkill3", hitPosition, rotation)
            .GetComponent<HitController>()
            .InitializeRound(
                damage,
                0f,         // 이동 없음
                0.3f,       // 판정 시간
                0f,         // 시작 딜레이 없음
                0.3f,       // 지속 시간
                0f,         // 다중 타격 없음
                player,
                player.faction,
                new Vector3(radius, 1f, radius),
                hitPosition,
                forward,
                1f,         // 반지름 비율 (스케일로 조절)
                0f,         // 내부 반지름 없음
                360f        // 전방향
            );
    }

    public void Update() { }

    public void Remove()
    {
        if (hitCallback != null)
        {
            player.OnAttackHit -= hitCallback;
            hitCallback = null;
        }
    }
}
