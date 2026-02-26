using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMoveSpeed : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
        {
            StatModifier cardModifier = new StatModifier
            {
                statType = StatType.MoveSpeed,
                type = ModifierType.Add,
                value = card.amountByMaxLevel * PlayerManager.Instance.GetPlayer().baseStatus.attackSpeed
            };

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
        else
        {
            StatModifier cardModifier = new StatModifier
            {
                statType = StatType.MoveSpeed,
                type = ModifierType.Add,
                value = card.amount * PlayerManager.Instance.GetPlayer().baseStatus.attackSpeed
            };

            PlayerManager.Instance.GetPlayer().AddModifier(cardModifier);
        }
    }
}
