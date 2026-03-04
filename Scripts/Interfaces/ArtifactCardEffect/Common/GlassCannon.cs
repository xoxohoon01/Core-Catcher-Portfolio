using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCannon : IArtifactCardEffect
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
                int level = CardManager.Instance
                    .artifactEffectLevel[card.effectName];

                if (level <= 0) return 0f;

                return card.GetValue(AttributeType.amount, level);
            },
            () => true
        ));

        player.AddModifier(new ConditionalModifier(
            StatType.MaxHP,
            ModifierType.Multiply,
            () =>
            {
                return -0.2f;
            },
            () => true
        ));
    }
}