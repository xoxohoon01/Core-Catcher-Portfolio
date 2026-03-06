using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoSingleton<CardManager>
{
    public Dictionary<string, int> levelUpEffectLevel = new();
    public Dictionary<string, int> artifactEffectLevel = new();

    private Dictionary<string, LevelUpCardScriptableObject> levelUpCards = new();
    private Dictionary<string, ArtifactCardScriptableObject> artifactCards = new();

    protected override void Awake()
    {
        base.Awake();

        levelUpEffectLevel.Clear();
        artifactEffectLevel.Clear();

        var levelUpAllCards = Resources.LoadAll<LevelUpCardScriptableObject>("Cards");
        foreach (var card in levelUpAllCards)
        {
            levelUpEffectLevel.Add(card.effectName, 0);
            levelUpCards.Add(card.effectName, card);
        }

        var artifacts = Resources.LoadAll<ArtifactCardScriptableObject>("Artifacts");
        foreach (var card in artifacts)
        {
            artifactEffectLevel.Add(card.effectName, 0);
            artifactCards.Add(card.effectName, card);
        }
    }

    public void SelectArtifact(string effectName)
    {
        artifactEffectLevel[effectName]++;

        ArtifactManager.Instance.OnArtifactLevelUp(effectName);
    }

    public void SelectLevelUp(string effectName)
    {
        levelUpEffectLevel[effectName]++;

        var effect = CreateLevelUpEffect(effectName);
        effect?.ApplyEffect(levelUpCards[effectName]);
    }

    public ILevelUpCardEffect CreateLevelUpEffect(string className)
    {
        Type type = Type.GetType(className);
        if (type == null) return null;

        return Activator.CreateInstance(type) as ILevelUpCardEffect;
    }

    public ArtifactCardScriptableObject GetArtifact(string effectName)
    {
        if (!artifactCards.TryGetValue(effectName, out var card))
        {
            Debug.LogWarning($"Artifact not found : {effectName}");
            return null;
        }

        return card;
    }
}