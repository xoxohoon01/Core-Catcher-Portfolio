using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamage : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
            PlayerManager.Instance.GetPlayer().status.damage += card.amountByMaxLevel;
        else
            PlayerManager.Instance.GetPlayer().status.damage += card.amount;
    }
}
