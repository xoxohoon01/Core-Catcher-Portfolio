using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : IStatCardEffect, IArtifactCardEffect
{
    public void Update()
    {
    }
    public void ApplyEffect(StatCardScriptableObject card)
    {
        PlayerManager.Instance.GetPlayer().GetHeal(PlayerManager.Instance.GetPlayer().status.maxHP * 0.5f);
    }

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        PlayerManager.Instance.GetPlayer().GetHeal(PlayerManager.Instance.GetPlayer().status.maxHP * 0.5f);
    }

    public void OnDash(Vector3 position, float damage, UnitController owner)
    {
    }
}
