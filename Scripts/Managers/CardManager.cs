using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoSingleton<CardManager>
{
    public Dictionary<string, int> statLevel = new();
    public Dictionary<string, int> artifactEffectLevel = new();

    private Dictionary<string, StatCardScriptableObject> statCards = new();
    private Dictionary<string, ArtifactCardScriptableObject> artifactCards = new();

    public int OwnedArtifactCount => artifactEffectLevel.Count(x => x.Value > 0);
    public const int MaxArtifactSlot = 4;

    public void SelectArtifact(string effectName)
    {
        artifactEffectLevel[effectName]++;

        ArtifactManager.Instance.OnArtifactLevelUp(effectName);
    }

    public void SelectLevelUp(string effectName)
    {
        statLevel[effectName]++;

        var effect = CreateLevelUpEffect(effectName);
        effect?.ApplyEffect(statCards[effectName]);
    }

    public IStatCardEffect CreateLevelUpEffect(string className)
    {
        Type type = Type.GetType(className);
        if (type == null) return null;

        return Activator.CreateInstance(type) as IStatCardEffect;
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

    protected override void Awake()
    {
        base.Awake();

        statLevel.Clear();
        artifactEffectLevel.Clear();

        var levelUpAllCards = Resources.LoadAll<StatCardScriptableObject>("CardSO");
        foreach (var card in levelUpAllCards)
        {
            statLevel.Add(card.effectName, 0);
            statCards.Add(card.effectName, card);
        }

        var artifacts = Resources.LoadAll<ArtifactCardScriptableObject>("ArtifactSO");
        foreach (var card in artifacts)
        {
            artifactEffectLevel.Add(card.effectName, 0);
            artifactCards.Add(card.effectName, card);
        }
    }
}