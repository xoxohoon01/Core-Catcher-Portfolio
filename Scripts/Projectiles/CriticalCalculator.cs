using DamageNumbersPro;
using UnityEngine;

public struct CritResult
{
    public int level;
    public float multiplier;
    public DamageNumber damageNumber;
}

public static class CriticalCalculator
{
    public static CritResult Calculate(UnitController attacker)
    {
        float crit = attacker.status.critChance * 100f;

        bool normalCrit = Random.Range(0f, 100f) < Mathf.Min(crit, 100f);
        crit -= 100f;

        bool superCrit = false;
        if (crit > 0)
        {
            superCrit = Random.Range(0f, 100f) < Mathf.Min(crit, 100f);
            crit -= 100f;
        }

        bool ultraCrit = false;
        if (crit > 0)
        {
            ultraCrit = Random.Range(0f, 100f) < Mathf.Min(crit, 100f);
        }

        CritResult result = new CritResult();

        if (ultraCrit)
        {
            result.level = 4;
            result.damageNumber = DamageNumberManager.Instance.ultraCritHitPrefab;
            result.multiplier = attacker.status.critDamage * 3f;
        }
        else if (superCrit)
        {
            result.level = 3;
            result.damageNumber = DamageNumberManager.Instance.superCritHitPrefab;
            result.multiplier = attacker.status.critDamage * 2f;
        }
        else if (normalCrit)
        {
            result.level = 2;
            result.damageNumber = DamageNumberManager.Instance.normalCritHitPrefab;
            result.multiplier = attacker.status.critDamage;
        }
        else
        {
            result.level = 1;
            result.damageNumber = DamageNumberManager.Instance.healthHitPrefab;
            result.multiplier = 1f;
        }

        return result;
    }
}