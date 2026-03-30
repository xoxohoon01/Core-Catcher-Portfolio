using System;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoSingleton<ArtifactManager>
{
    private Dictionary<string, IArtifactCardEffect> activeEffects = new();
    private Dictionary<string, ArtifactCardScriptableObject> artifactCards = new();

    public void OnArtifactLevelUp(string effectName)
    {
        if (activeEffects.TryGetValue(effectName, out var existingEffect))
        {
            return;
        }

        // »õ·Î È¹µæÇÑ ¾ÆÆ¼ÆÑÆ®¶ó¸é »ý¼º
        CreateEffect(effectName);
    }

    private void CreateEffect(string effectName)
    {
        Type type = Type.GetType(effectName);
        if (type == null)
            return;

        var effect = Activator.CreateInstance(type) as IArtifactCardEffect;
        if (effect != null)
        {
            effect.ApplyEffect(artifactCards[effectName]);
            activeEffects[effectName] = effect;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        var cards = Resources.LoadAll<ArtifactCardScriptableObject>("Artifacts");
        foreach (var card in cards)
        {
            artifactCards[card.effectName] = card;
        }
    }

    private void Update()
    {
        if (BattleManager.Instance != null && BattleManager.Instance.isStop) return;

        foreach (var effect in activeEffects.Values)
            effect.Update();
    }
}