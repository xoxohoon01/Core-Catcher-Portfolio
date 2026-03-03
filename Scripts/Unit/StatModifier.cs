using UnityEngine;

public enum StatType
{
    MaxHP,
    Damage,
    MoveSpeed,
    AttackSpeed,
    Armor,
    CooldownReduction,
    SkillRange,
    SkillSpeed,
    CritChance,
    CritDamage,
    SkillDamage,
    Drain,
    DashCooldown
}

public enum ModifierType
{
    Add,
    Multiply
}

public interface IStatModifier
{
    public StatType StatType { get; }
    public ModifierType ModifierType { get; }
    float GetValue();
    bool IsExpired();
}


public class StatModifier : IStatModifier
{
    public StatType StatType { get; private set; }
    public ModifierType ModifierType { get; private set; }

    private float value;
    private float duration;
    private float startTime;

    public StatModifier(StatType statType, ModifierType type, float value, float duration = 0f)
    {
        StatType = statType;
        ModifierType = type;
        this.value = value;
        this.duration = duration;
        startTime = Time.time;
    }

    public float GetValue()
    {
        return value;
    }

    public bool IsExpired()
    {
        if (duration <= 0f) return false;
        return Time.time > startTime + duration;
    }
}

public class ConditionalModifier : IStatModifier
{
    public StatType StatType { get; private set; }
    public ModifierType ModifierType { get; private set; }

    private System.Func<float> valueFunc;
    private System.Func<bool> conditionFunc;

    private float duration;
    private float startTime;

    public ConditionalModifier(
        StatType statType,
        ModifierType type,
        System.Func<float> valueFunc,
        System.Func<bool> conditionFunc,
        float duration = 0f)
    {
        StatType = statType;
        ModifierType = type;
        this.valueFunc = valueFunc;
        this.conditionFunc = conditionFunc;
        this.duration = duration;
        startTime = Time.time;
    }

    public float GetValue()
    {
        if (conditionFunc == null || conditionFunc())
            return valueFunc();

        return 0f;
    }

    public bool IsExpired()
    {
        if (duration <= 0f) return false;
        return Time.time >= startTime + duration;
    }
}