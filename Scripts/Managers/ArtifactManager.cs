using System;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoSingleton<ArtifactManager>
{
    private Dictionary<string, IArtifactCardEffect> activeEffects = new();
    private Dictionary<string, ArtifactCardScriptableObject> artifactCards = new();

    protected override void Awake()
    {
        base.Awake();

        var cards = Resources.LoadAll<ArtifactCardScriptableObject>("Artifacts");
        foreach (var card in cards)
        {
            artifactCards[card.effectName] = card;
        }
    }

    public void OnArtifactLevelUp(string effectName)
    {
        int level = CardManager.Instance.artifactEffectLevel[effectName];

        if (level == 1)
            CreateEffect(effectName);
    }

    private void CreateEffect(string effectName)
    {
        Type type = Type.GetType(effectName);
        if (type == null)
        {
            Debug.LogWarning($"Artifact class '{effectName}' not found.");
            return;
        }

        var effect = Activator.CreateInstance(type) as IArtifactCardEffect;
        effect.ApplyEffect(artifactCards[effectName]);

        activeEffects[effectName] = effect;
    }

    private void Update()
    {
        foreach (var effect in activeEffects.Values)
            effect.Update();
    }
}