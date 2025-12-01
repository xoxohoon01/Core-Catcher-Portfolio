using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSkillSpeed : ILevelUpCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        if (CardManager.Instance.levelUpEffectLevel[ToString()] == 5)
            PlayerManager.Instance.GetPlayer().status.skillSpeed += card.amountByMaxLevel;
        else
            PlayerManager.Instance.GetPlayer().status.skillSpeed += card.amount;
    }
}
