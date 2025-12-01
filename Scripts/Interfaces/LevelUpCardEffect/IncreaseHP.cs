using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHP : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
        {
            PlayerManager.Instance.GetPlayer().status.maxHP += card.amountByMaxLevel;
            PlayerManager.Instance.GetPlayer().GetHeal(card.amountByMaxLevel);
        }
        else
        {
            PlayerManager.Instance.GetPlayer().status.maxHP += card.amount;
            PlayerManager.Instance.GetPlayer().GetHeal(card.amount);
        }
    }
}
