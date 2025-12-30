using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealHalf : ILevelUpCardEffect, IArtifactCardEffect
{
    public void ApplyEffect(LevelUpCardScriptableObject card)
    {
        PlayerManager.Instance.GetPlayer().GetHeal(PlayerManager.Instance.GetPlayer().status.maxHP * 0.5f);
    }

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        PlayerManager.Instance.GetPlayer().GetHeal(PlayerManager.Instance.GetPlayer().status.maxHP * 0.5f);
    }
}
