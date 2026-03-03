using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamage : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.Damage,
                ModifierType.Add,
                card.amountByMaxLevel
            );

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
        else
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.Damage,
                ModifierType.Add,
                card.amount
            );

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
    }
}
