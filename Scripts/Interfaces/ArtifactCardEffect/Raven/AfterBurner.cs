using UnityEngine;

// N번 명중 시 부채꼴 방향으로 RavenAttack 탄환을 일제히 발사
// 탄환은 RavenAttackController를 사용하므로 FlyingBullet, Targeting 아티팩트가 자동 적용됨
public class AfterBurner : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private System.Action<UnitController, Vector3> hitCallback;

    // 현재 명중 횟수 카운터
    private int hitCount = 0;

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        hitCallback = OnAttackHit;
        player.OnAttackHit += hitCallback;
    }

    private void OnAttackHit(UnitController target, Vector3 hitPosition)
    {
        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        if (level <= 0) return;

        hitCount++;

        int requiredHits = Mathf.RoundToInt(card.GetValue(AttributeType.period, level));

        if (hitCount < requiredHits) return;

        // 카운터 초기화 후 발사
        hitCount = 0;
        FireBurst(level);
    }

    private void FireBurst(int level)
    {
        int bulletCount = Mathf.RoundToInt(card.GetValue(AttributeType.amount, level));
        // 탄환 수가 많으므로 개당 데미지는 기본 공격의 80%로 고정
        float damage = player.status.damage * 0.8f;

        // 캐릭터가 바라보는 방향을 중심으로 부채꼴 발사
        Vector3 baseDir = player.transform.forward;
        baseDir.y = 0;
        baseDir.Normalize();

        // 탄환 수에 따라 좌우로 30도 간격으로 퍼지도록 각도 계산
        float spreadAngle = 30f;
        float totalSpread = spreadAngle * (bulletCount - 1);
        float startAngle = -totalSpread / 2f;

        Vector3 spawnPos = player.transform.position + (Vector3.up * 2f);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + (spreadAngle * i);
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;
            Quaternion rotation = Quaternion.LookRotation(dir);

            // RavenAttackController 사용 — FlyingBullet, Targeting 자동 적용
            ObjectPoolManager.Instance
                .Spawn("RavenAttack", spawnPos, rotation)
                .GetComponent<RavenAttackController>()
                .Initialize(damage, player.CheckCritical(), 100f, 1f, false, player, player.faction, Vector3.one);
        }
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
