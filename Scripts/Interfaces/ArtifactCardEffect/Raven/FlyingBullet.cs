using System.Collections.Generic;
using UnityEngine;

public class FlyingBullet : IArtifactCardEffect
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

        // 명중한 적 주변의 다른 적을 탐색
        float searchRadius = card.GetValue(AttributeType.amount, level);
        Collider[] nearbyColliders = Physics.OverlapSphere(hitPosition, searchRadius, LayerMask.GetMask("Monster"));

        // 명중한 대상을 제외한 가장 가까운 적 탐색
        UnitController nearestTarget = null;
        float nearestDist = float.MaxValue;

        foreach (var col in nearbyColliders)
        {
            UnitController unit = col.GetComponent<UnitController>();
            if (unit == null || unit == hitTarget || unit.isDead) continue;

            float dist = Vector3.Distance(hitPosition, unit.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestTarget = unit;
            }
        }

        // 주변 적이 없으면 원래 명중한 적 방향으로 발사
        Vector3 targetPos = nearestTarget != null
            ? nearestTarget.transform.position
            : hitPosition;

        Vector3 dir = (targetPos - player.transform.position);
        dir.y = 0;
        if (dir == Vector3.zero) return;

        Quaternion rotation = Quaternion.LookRotation(dir.normalized);
        Vector3 spawnPos = player.transform.position + (player.transform.forward) + (Vector3.up * 2f);

        // 추가 탄환 발사 — 2차 탄환이므로 OnAttackHit 이벤트 발생 안 함
        float bulletDamage = player.status.damage * card.GetValue(AttributeType.duration, level);
        BulletController bullet = ObjectPoolManager.Instance
            .Spawn("RavenAttack", spawnPos, rotation)
            .GetComponent<BulletController>();
        bullet.invokeAttackHitEvent = false;
        bullet.Initialize(bulletDamage, player.CheckCritical(), 100f, 1f, false, player, player.faction, Vector3.one);
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
