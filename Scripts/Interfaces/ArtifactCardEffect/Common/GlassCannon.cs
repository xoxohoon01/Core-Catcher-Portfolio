using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GlassCannon : IArtifactCardEffect
{
    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        PlayerController player = PlayerManager.Instance.GetPlayer();

        player.AddModifier(new ConditionalModifier(
            StatType.Damage,
            ModifierType.Multiply,
            () =>
            {
                int level = CardManager.Instance.artifactEffectLevel[card.effectName];

                float value = card.baseAmount +
                                 (card.amountPerLevel * (level - 1)) +
                                 (level == 5 ? card.amountByMaxLevel : 0);

                return value;
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
