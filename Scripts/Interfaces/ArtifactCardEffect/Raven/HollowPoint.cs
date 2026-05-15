using UnityEngine;

// 크리티컬 발생 시 명중한 적의 방어력을 일정 시간 감소시킴
public class HollowPoint : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private System.Action<int> critCallback;

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        critCallback = OnCritical;
        player.OnCritical += critCallback;
    }

    private void OnCritical(int critLevel)
    {
        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        if (level <= 0) return;

        // 크리 발생 시점의 가장 가까운 적에게 방어력 감소 적용
        Collider[] nearbyColliders = Physics.OverlapSphere(player.transform.position, 30f, LayerMask.GetMask("Monster"));

        float minDist = float.MaxValue;
        UnitController nearestTarget = null;

        foreach (var col in nearbyColliders)
        {
            UnitController unit = col.GetComponent<UnitController>();
            if (unit == null || unit.isDead) continue;

            float dist = Vector3.Distance(player.transform.position, unit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestTarget = unit;
            }
        }

        if (nearestTarget == null) return;

        float armorReduction = card.GetValue(AttributeType.amount, level);
        float duration = card.GetValue(AttributeType.duration, level);

        // 방어력 감소 수정자 추가 (음수 가산)
        nearestTarget.AddModifier(new StatModifier(
            StatType.Armor,
            ModifierType.Add,
            -armorReduction,
            duration
        ));
    }

    public void Update() { }

    public void Remove()
    {
        if (critCallback != null)
        {
            player.OnCritical -= critCallback;
            critCallback = null;
        }
    }
}
