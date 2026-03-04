using UnityEngine;

public class Berserk : IArtifactCardEffect
{
    public void Update()
    {
    }

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        PlayerController player = PlayerManager.Instance.GetPlayer();

        player.AddModifier(new ConditionalModifier(
            StatType.Damage,
            ModifierType.Multiply,
            () =>
            {
                if (player.status.maxHP <= 0)
                    return 0f;

                float ratio = player.status.hp / player.status.maxHP;

                if (ratio >= 0.8f)
                    return 0f;

                float t = (0.8f - ratio) / 0.7f;
                t = Mathf.Clamp01(t);

                int level = CardManager.Instance
                    .artifactEffectLevel[card.effectName];

                if (level <= 0)
                    return 0f;

                float maxValue = card.GetValue(AttributeType.amount, level);

                return maxValue * t;
            },
            () => true
        ));
    }
}