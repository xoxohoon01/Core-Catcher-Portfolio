using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct WeightedItem<T>
{
    public T item;
    public int weight;
    public bool isEssential;
}

public static class WeightedRandom
{
    public static T Choose<T>(List<WeightedItem<T>> items, List<T> excludeItems = null)
    {
        int totalWeight = items.Sum(i => i.weight);
        int roll = Random.Range(0, totalWeight);
        int current = 0;

        // exclude Á¦°Å
        if (excludeItems != null)
        {
            foreach (var i in items)
            {
                if (excludeItems.Contains(i.item))
                    continue;
                totalWeight += i.weight;
            }
        }
        

        foreach (var entry in items)
        {
            current += entry.weight;
            if (roll < current)
                return entry.item;
        }

        // fallback (should never happen if weights > 0)
        return items.Last().item;
    }

    public static List<T> ChooseAll<T>(List<WeightedItem<T>> items)
    {
        List<T> list = new List<T>();

        int totalWeight = items.Sum(i => i.weight);
        int roll = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var entry in items)
        {
            current += entry.weight;
            if (roll < current)
            {
                list.Add(entry.item);
            }
        }

        return list;
    }
}
