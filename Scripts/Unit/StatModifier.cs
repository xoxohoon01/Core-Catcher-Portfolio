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

public class StatModifier
{
    public StatType statType;
    public ModifierType type;
    public float value;
    public float duration;
    public object source;
}