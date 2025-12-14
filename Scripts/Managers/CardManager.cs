using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoSingleton<CardManager>
{
    public int levelUpCount = 0;
    public Dictionary<string, int> levelUpEffectLevel = new Dictionary<string, int>();
    public Dictionary<string, int> artifactEffectLevel = new Dictionary<string, int>();

    public ILevelUpCardEffect CreateLevelUpEffect(string className)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            Debug.LogWarning($"클래스 '{className}'을 찾을 수 없습니다.");
            return null;
        }

        if (!typeof(ILevelUpCardEffect).IsAssignableFrom(type))
        {
            Debug.LogWarning($"'{className}'은 ICardEffect를 구현하지 않았습니다.");
            return null;
        }

        return Activator.CreateInstance(type) as ILevelUpCardEffect;
    }

    public IArtifactCardEffect CreateArtifactEffect(string className)
    {
        Type type = Type.GetType(className);
        if (type == null)
        {
            Debug.LogWarning($"클래스 '{className}'을 찾을 수 없습니다.");
            return null;
        }

        if (!typeof(IArtifactCardEffect).IsAssignableFrom(type))
        {
            Debug.LogWarning($"'{className}'은 ICardEffect를 구현하지 않았습니다.");
            return null;
        }

        return Activator.CreateInstance(type) as IArtifactCardEffect;
    }

    protected override void Awake()
    {
        base.Awake();

        levelUpEffectLevel.Clear();
        artifactEffectLevel.Clear();

        LevelUpCardScriptableObject[] levelUpAllCards = Resources.LoadAll<LevelUpCardScriptableObject>("Cards");
        foreach (LevelUpCardScriptableObject card in levelUpAllCards)
        {
            levelUpEffectLevel.Add(card.effectName, 0);
        }

        ArtifactCardScriptableObject[] artifactAllCards = Resources.LoadAll<ArtifactCardScriptableObject>("Artifacts");
        foreach (ArtifactCardScriptableObject card in artifactAllCards)
        {
            artifactEffectLevel.Add(card.effectName, 0);
        }

        artifactEffectLevel["FlyingBullet"] = 1;
    }
}
