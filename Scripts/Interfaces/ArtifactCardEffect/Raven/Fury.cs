using UnityEngine;

public class Fury : IArtifactCardEffect
{
    private ConditionalModifier critModifier;
    private float currentBonus = 0f;
    private float maxBonus;
    private float increasePerUse;
    private float decayDelay = 5f;
    private float decayTimer = 0f;

    private PlayerController player;
    private System.Action skillCallback;

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        player = PlayerManager.Instance.GetPlayer();

        critModifier = new ConditionalModifier(
            StatType.CritChance,
            ModifierType.Add,
            () => currentBonus,
            () => true
        );

        player.AddModifier(critModifier);

        skillCallback = () => OnSkillUse(card);
        player.OnSkillUsed += skillCallback;
    }

    private void OnSkillUse(ArtifactCardScriptableObject card)
    {
        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        maxBonus = card.baseAmount +
            ((level - 1) * card.amountPerLevel) +
            (level == 5 ? card.amountByMaxLevel : 0);

        increasePerUse = maxBonus / 10f;

        currentBonus += increasePerUse;
        currentBonus = Mathf.Min(currentBonus, maxBonus);

        decayTimer = 0f; // 스킬 사용 시 초기화 타이머 리셋
        player.isDirty = true;
    }

    public void Update()
    {
        if (currentBonus <= 0f) return;

        decayTimer += Time.deltaTime;
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