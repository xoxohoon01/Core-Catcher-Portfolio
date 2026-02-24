using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAttackSpeed : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
            PlayerManager.Instance.GetPlayer().status.attackSpeed += PlayerManager.Instance.GetPlayer().characterData.attackSpeed * card.amountByMaxLevel;
        else
            PlayerManager.Instance.GetPlayer().status.attackSpeed += PlayerManager.Instance.GetPlayer().characterData.attackSpeed * card.amount;
    }
}
