using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseCooldown : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
        {
            StatModifier cardModifier = new StatModifier
            {
                statType = StatType.CooldownReduction,
                type = ModifierType.Add,
                value = card.amountByMaxLevel
            };

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
        else
        {
            StatModifier cardModifier = new StatModifier
            {
                statType = StatType.CooldownReduction,
                type = ModifierType.Add,
                value = card.amount
            };

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
    }
}
