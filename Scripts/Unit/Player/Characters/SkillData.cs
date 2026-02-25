using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    [Header("Basic Info")]
    public string skillID;          // 내부 식별용 (ex: "raven_peacemaker")
    public string animationClipName;// 클립 이름

    [TextArea]
    public string displayName;      // UI 표시 이름
    public string description;      // 설명

    [Header("Stats")]
    public float baseCooldown;
    public float baseSpan;

    public virtual void OnSkillStart(UnitController user) { }
    public virtual void OnSkillCancel(UnitController user) { }
    public virtual void OnAnimationEvent(UnitController user, int number = 0) { }
    public virtual void OnSkillEnd(UnitController user) { }
}