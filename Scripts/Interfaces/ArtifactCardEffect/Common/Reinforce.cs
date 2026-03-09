using UnityEngine;

public class Reinforce : IArtifactCardEffect
{
    public void Update()
    {
    }

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        PlayerController player = PlayerManager.Instance.GetPlayer();

        player.AddModifier(new ConditionalModifier(
            StatType.SkillDamage,
            ModifierType.Multiply,
            () =>
            {
                int level = CardManager.Instance
                    .artifactEffectLevel[card.effectName];

                if (level <= 0) return 0f;

                return card.GetValue(AttributeType.amount, level);
            },
            () => true
        ));
    }
}