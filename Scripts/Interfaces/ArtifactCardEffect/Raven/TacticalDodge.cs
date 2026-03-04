using UnityEngine;

public class TacticalDodge : IArtifactCardEffect
{
    private PlayerController player;
    private ConditionalModifier critModifier;
    private System.Action dashCallback;

    private ArtifactCardScriptableObject card;

    private float dodgeBonus = 0f;
    private float decayTimer = 0f;

    public void ApplyEffect(ArtifactCardScriptableObject card)
    {
        this.card = card;
        player = PlayerManager.Instance.GetPlayer();

        critModifier = new ConditionalModifier(
            StatType.CritChance,
            ModifierType.Add,
            () => dodgeBonus,
            () => dodgeBonus > 0f
        );

        player.AddModifier(critModifier);

        dashCallback = OnDash;
        player.OnDash += dashCallback;
    }

    private void OnDash()
    {
        int level = CardManager.Instance
            .artifactEffectLevel[card.effectName];

        if (level <= 0) return;

        dodgeBonus = card.GetValue(AttributeType.amount, level);

        decayTimer = 0f;
        player.isDirty = true;
    }

    public void Update()
    {
        if (dodgeBonus <= 0f) return;

        decayTimer += Time.deltaTime;

        float duration = card.GetValue(AttributeType.duration,
            CardManager.Instance.artifactEffectLevel[card.effectName]);

        if (decayTimer >= duration)
        {
            dodgeBonus = 0f;
            decayTimer = 0f;
            player.isDirty = true;
        }
    }

    public void Remove()
    {
        if (dashCallback != null)
        {
            player.OnDash -= dashCallback;
            dashCallback = null;
        }
    }
}