using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSkillSpeed : IStatCardEffect
{
    public void ApplyEffect(StatCardScriptableObject card)
    {
        if (CardManager.Instance.statLevel[ToString()] == 5)
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.SkillSpeed,
                ModifierType.Add,
                card.amountByMaxLevel
            );

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
        else
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.SkillSpeed,
                ModifierType.Add,
                card.amount
            );

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
    }
}
