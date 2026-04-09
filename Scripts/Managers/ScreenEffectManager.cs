using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectManager : MonoSingleton<ScreenEffectManager>
{
    private Dictionary<string, ScreenEffect> effectDictionary;

    protected override void Awake()
    {
        base.Awake();

        effectDictionary = new Dictionary<string, ScreenEffect>();
        foreach (Transform child in transform)
        {
            child.TryGetComponent(out ScreenEffect targetEffect);
            effectDictionary.Add(targetEffect.effectName, targetEffect);
        }
    }
        
    public void BeginEffect(string effectName, float time = 1)
    {
        if (effectDictionary.ContainsKey(effectName))
        {
            effectDictionary[effectName].BeginEffect(time);
        }
    }
}
