using UnityEngine;

public class Fury : IArtifactCardEffect
{
    private PlayerController player;
    private ConditionalModifier critModifier;
    private System.Action skillCallback;

    private ArtifactCardScriptableObject card;

    private float currentBonus = 0f;
    private float maxBonus;
    private float increasePerUse;
    private float decayTimer = 0f;

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        critModifier = new ConditionalModifier(
            StatType.CritChance,
            ModifierType.Add,
            () => currentBonus,
            () => true
        );

        player.AddModifier(critModifier);

        skillCallback = OnSkillUse;
        player.OnSkillUsed += skillCallback;
    }

    private void OnSkillUse()
    {
        int level = CardManager.Instance
            .artifactEffectLevel[card.effectName];

        if (level <= 0) return;

        maxBonus = card.GetValue(AttributeType.amount, level);

        increasePerUse = maxBonus / 10f;

        currentBonus += increasePerUse;
        currentBonus = Mathf.Min(currentBonus, maxBonus);

        decayTimer = 0f;
        player.isDirty = true;
    }

    public void Update()
    {
        if (currentBonus <= 0f) return;

        decayTimer += Time.deltaTime;

        float decayDelay = card.GetValue(AttributeType.duration,
            CardManager.Instance.artifactEffectLevel[card.effectName]);

        if (decayTimer >= decayDelay)
        {
            currentBonus = 0f;
            player.isDirty = true;
            decayTimer = 0f;
        }
    }

    public void Remove()
    {
        if (skillCallback != null)
        {
            player.OnSkillUsed -= skillCallback;
            skillCallback = null;
        }
    }
}