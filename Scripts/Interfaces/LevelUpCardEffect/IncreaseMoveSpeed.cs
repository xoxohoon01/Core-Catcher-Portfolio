using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMoveSpeed : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
            PlayerManager.Instance.GetPlayer().status.moveSpeed += card.amountByMaxLevel;
        else
            PlayerManager.Instance.GetPlayer().status.moveSpeed += card.amount;
    }
}
