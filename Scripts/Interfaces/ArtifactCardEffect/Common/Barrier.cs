using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : IArtifactCardEffect
{
    private PlayerController player;
    private ArtifactCardScriptableObject card;
    private float decayTimer = 0f;

    public void Update()
    {
        if (player == null) return;
        if (player.status.shield > 0) return;

        int level = CardManager.Instance
            .artifactEffectLevel[card.effectName];

        if (level <= 0) return;

        decayTimer += Time.deltaTime;

        float decayDelay = card.GetValue(AttributeType.period, level);

        if (decayTimer >= decayDelay)
        {
            float shieldAmount = player.status.maxHP * card.GetValue(AttributeType.amount, level);

            player.status.shield = shieldAmount;
            decayTimer = 0f;
        }
    }

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();
    }
}