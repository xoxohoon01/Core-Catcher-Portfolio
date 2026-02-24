using Microlight.MicroBar;
using PixPlays.ElementalVFX;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum Faction
{
    Player,
    Monster
};

public class UnitController : MonoBehaviour
{
    protected new Rigidbody rigidbody;
    public MicroBar healthBar;

    public Animator animator { get; protected set; }

    public bool isKnockback {  get; protected set; }
    public float knockbackTime { get; protected set; }
    public float knockbackSpan { get; protected set; }

    public bool isAirborne { get; protected set; }
    public float airborneSpan { get; protected set; }

    public bool isDead { get; protected set; }

    public Vector3 moveVector;

    protected Vector3 knockbackVector;
    protected Vector3 knockbackStartVector;
    protected Vector3 airborneVector;

    public Faction faction;
    public BaseStatus baseStatus;
    public Status status = new Status();
    protected List<StatModifier> modifiers = new List<StatModifier>();
    protected bool isDirty = true;

    protected Vector3 lastVelocity;
    protected float lastAnimSpeed;

    public bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.2f, transform.lossyScale.z);
        return Physics.CheckBox(transform.position, boxSize, Quaternion.identity, LayerMask.GetMask("Ground"));
    }

    public virtual void GetKnockback(Vector3 directionVector, float knockbackForce, float knockbackTime)
    {
        isKnockback = true;
        this.knockbackTime = knockbackTime;
        knockbackSpan = 0;

        moveVector = Vector3.zero;
        knockbackStartVector = directionVector.normalized * knockbackForce;
        knockbackStartVector.y = 0;
        knockbackVector = knockbackStartVector;

        transform.rotation = Quaternion.LookRotation(transform.position - directionVector, Vector3.up);
    }

    public virtual void GetAirBorne(float airborneForce)
    {
        isAirborne = true;
        airborneSpan = 0;

        moveVector = Vector3.zero;
        airborneVector = new Vector3(0, airborneForce, 0);
    }

    public void GetHeal(float healAmount)
    {
        status.hp = Mathf.Min(status.hp + healAmount, status.maxHP);
    }

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        isDirty = true;
    }

    void UpdateModifiers()
    {
        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            if (modifiers[i].duration > 0)
            {
                modifiers[i].duration -= Time.deltaTime;
                if (modifiers[i].duration <= 0)
                {
                    modifiers.RemoveAt(i);
                    isDirty = true;
                }
            }
        }
    }

    protected float CalculateStat(StatType type, float baseValue)
    {
        float add = 0f;
        float mul = 0f;

        foreach (var mod in modifiers)
        {
            if (mod.statType != type) continue;

            if (mod.type == ModifierType.Add)
                add += mod.value;
            else
                mul += mod.value;
        }

        return (baseValue + add) * (1f + mul);
    }

    protected void RecalculateStats()
    {
        if (!isDirty) return;

        status.maxHP = CalculateStat(StatType.MaxHP, baseStatus.maxHP);
        status.armor = CalculateStat(StatType.Armor, baseStatus.armor);
        status.damage = CalculateStat(StatType.Damage, baseStatus.damage);
        status.moveSpeed = CalculateStat(StatType.MoveSpeed, baseStatus.moveSpeed);
        status.attackSpeed = CalculateStat(StatType.AttackSpeed, baseStatus.attackSpeed);
        status.cooldownReduction = CalculateStat(StatType.CooldownReduction, baseStatus.cooldownReduction);
        status.skillRange = CalculateStat(StatType.SkillRange, baseStatus.skillRange);
        status.skillSpeed = CalculateStat(StatType.SkillSpeed, baseStatus.skillSpeed);

        status.critChance = CalculateStat(StatType.CritChance, baseStatus.critChance);
        status.critDamage = CalculateStat(StatType.CritDamage, baseStatus.critDamage);
        status.skillDamage = CalculateStat(StatType.SkillDamage, baseStatus.skillDamage);
        status.drain = CalculateStat(StatType.Drain, baseStatus.drain);
        status.dashCooldown = CalculateStat(StatType.DashCooldown, baseStatus.dashCooldown);

        status.hp = Mathf.Min(status.hp, status.maxHP);

        isDirty = false;
    }

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (healthBar != null)
        {
            healthBar.transform.forward = Camera.main.transform.forward;
        }

        if (BattleManager.Instance.isStop)
        {
            return;
        }

        UpdateModifiers();
        RecalculateStats();

        if (isDead)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            rigidbody.velocity = Vector3.zero;
            return;
        }

        if (isKnockback)
        {
            knockbackSpan += Time.deltaTime;

            float knockbackForce = (knockbackSpan / knockbackTime);
            if (isAirborne)
            {
                knockbackVector = Vector3.Lerp(knockbackVector, knockbackStartVector * 0.2f, Time.deltaTime * 2f);
            }
            else
            {
                knockbackVector = Vector3.Lerp(knockbackStartVector, Vector3.zero, knockbackForce);
            }

            if (knockbackVector.magnitude <= 0)
            {
                isKnockback = false;
            }
        }

        if (isAirborne)
        {
            airborneSpan += Time.deltaTime;

            if (airborneSpan >= 0.1f)
            {
                airborneVector.y = Mathf.Max(airborneVector.y - Time.deltaTime * 30f, -20f);
            }

            if (airborneVector.y < 0)
            {
                if (IsGrounded())
                {
                    isAirborne = false;
                    airborneVector = Vector3.zero;

                    // ¶¥¿¡ ´ê´Â ¼ø°£ ³Ë¹é ¾àÈ­
                    knockbackVector *= 0.3f;
                }
            }
            
        }
    }

    protected virtual void FixedUpdate()
    {
        if (BattleManager.Instance.isStop)
        {
            rigidbody.velocity = Vector3.zero;
            animator.speed = 0;
        }
        else if (!isDead)
        {
            if (animator.speed != 0) lastAnimSpeed = animator.speed;
            if (rigidbody.velocity.magnitude != 0) lastVelocity = rigidbody.velocity;

            animator.speed = lastAnimSpeed;
            rigidbody.velocity =
                new Vector3(isKnockback? knockbackVector.x : moveVector.x,
                isAirborne ? airborneVector.y : lastVelocity.y,
                isKnockback ? knockbackVector.z : moveVector.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.2f, transform.lossyScale.z);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
