using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAttackSpeed : ILevelUpCardEffect
{
    public void ApplyEffect(StatCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.AttackSpeed,
                ModifierType.Add,
                card.amountByMaxLevel * PlayerManager.Instance.GetPlayer().baseStatus.attackSpeed
            );

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
        else
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.AttackSpeed,
                ModifierType.Add,
                card.amount * PlayerManager.Instance.GetPlayer().baseStatus.attackSpeed
            );

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
    }
}
