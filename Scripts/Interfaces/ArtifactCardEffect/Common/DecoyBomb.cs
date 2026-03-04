using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyBomb : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private System.Action dashCallback;

    public void Update()
    {
    }

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        dashCallback = OnDash;
        player.OnDash += dashCallback;
    }

    public void OnDash()
    {
        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        if (level <= 0) return;

        float bombDamage = card.GetValue(AttributeType.amount, level);

        ObjectPoolManager.Instance
            .Spawn("DecoyBomb", player.transform.position, player.transform.rotation)
            .GetComponent<DecoyBombController>()
            .Initialize(
                player.status.damage * bombDamage,
                0,
                0.2f,
                0.5f,
                1.5f,
                0,
                player,
                Faction.Player,
                Vector3.one * 5f
            );
    }
}
