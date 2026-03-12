using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHP : IStatCardEffect
{
    PlayerController player;
    public void ApplyEffect(StatCardScriptableObject card)
    {
        if (CardManager.Instance.statLevel[ToString()] == 5)
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.MaxHP,
                ModifierType.Add,
                card.amountByMaxLevel
            );

            player = PlayerManager.Instance.GetPlayer();
            player.AddModifier(cardModifier);
            player.status.hp += card.amountByMaxLevel;
        }
        else
        {
            StatModifier cardModifier = new StatModifier
            (
                StatType.MaxHP,
                ModifierType.Add,
                card.amount
            );

            player = PlayerManager.Instance.GetPlayer();
            player.AddModifier(cardModifier);
            player.status.hp += card.amount;
        }
    }
}
