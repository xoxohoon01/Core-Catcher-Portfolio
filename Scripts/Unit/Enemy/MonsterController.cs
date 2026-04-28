using Microlight.MicroBar;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum MonsterState { Chase, Attack, Die }

public abstract class MonsterController : UnitController
{
    [Header("References")]
    public List<GameObject> modelObjects = new List<GameObject>();
    public BoxIndicator boxIndicator;
    public RoundIndicator roundIndicator;
    public LayerMask excludeMaskInAttack;
    public bool isBoss;

    [Header("FSM")]
    [HideInInspector] public Transform Target; // 추적 대상 (Player)
    [HideInInspector] public MonsterStateMachine StateMachine;
    [HideInInspector] public IMonsterState ChaseState;
    [HideInInspector] public IMonsterState AttackState;
    [HideInInspector] public NavMeshAgent agent;
    public abstract string AttackAnimationName { get; }

    private List<Material> materials = new List<Material>();
    private float dissapearAmount;

    [HideInInspector] public bool isAttack;
    public float attackDelay;
    [HideInInspector] public float attackRange;

    protected void SetupStatus(int level)
    {
        status.level = level;
        status.hp = status.maxHP;

        if (!isBoss)
        {
            status.maxHP = status.maxHP * (1.0f + ((status.level - 1) * 0.3f));
            status.damage = status.damage * (1.0f + ((status.level - 1) * 0.1f));
            status.armor = status.armor * (1.0f + ((status.level - 1) * 0.25f));
        }
    }

    public void Initialize(int level)
    {
        // NavMesh Agent 설정
        agent.enabled = true;

        // 생성 파티클
        MonsterAppearance appearance = ObjectPoolManager.Instance.Spawn("MonsterAppearance", transform.position, Quaternion.identity).GetComponent<MonsterAppearance>();
        appearance.Initialize(3f);
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.enabled = true;
        appearance.transform.localScale = new Vector3(collider.bounds.size.x, collider.bounds.size.y, collider.bounds.size.z);

        // 디스폰 효과
        materials.Clear();
        foreach (GameObject modelObject in modelObjects)
        {
            Material material = new Material(modelObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial);
            materials.Add(material);
            modelObject.GetComponent<SkinnedMeshRenderer>().material = material;
        }

        dissapearAmount = 0;
        foreach (Material material in materials)
        {
            material.SetFloat("_Dissolve", dissapearAmount);
        }

        // 능력치 설정
        isDirty = true;
        RecalculateStats();
        SetupStatus(level);

        // 상태 설정
        isAttack = false;

        InitializeStateMachine();
        healthBar = ObjectPoolManager.Instance.Spawn("HealthBar", transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<MicroBar>();
        healthBar.Initialize(status.hp);
    }

    public virtual void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();
        ChaseState = new MonsterChaseState<MonsterController>(this);
        AttackState = new MonsterAttackState<MonsterController>(this);

        StateMachine.ChangeState(ChaseState);
    }

    private void ClearIndicators()
    {
        if (boxIndicator != null)
        {
            ObjectPoolManager.Instance.Despawn(boxIndicator.gameObject);
            boxIndicator = null;
        }
        if (roundIndicator != null)
        {
            ObjectPoolManager.Instance.Despawn(roundIndicator.gameObject);
            roundIndicator = null;
        }
    }

    public void CheckDeath()
    {
        if (!isDead && status.hp <= 0)
        {
            isDead = true;

            animator.Play($"{gameObject.name}Death");

            GetComponent<Collider>().enabled = false;
            agent.enabled = false;
            StateMachine = null;
            Vector3 dropPos = new Vector3(transform.position.x, 0, transform.position.z);

            float rand = Random.value; // 0.0 ~ 1.0

            if (rand <= 0.003f)
            {
                ObjectPoolManager.Instance.Spawn("Magnet", dropPos, transform.rotation);
            }
            else if (rand <= 0.002f)
            {
                ObjectPoolManager.Instance.Spawn("Medikit", dropPos, transform.rotation);
            }
            else
            {
                ObjectPoolManager.Instance.Spawn("ExpGem1", dropPos, transform.rotation);
            }
        }
        else if (BattleManager.Instance.entireTime >= BattleManager.Instance.stageData.bossTime && !isDead && !isBoss)
        {
            isDead = true;

            animator.Play($"{gameObject.name}Death");

            GetComponent<Collider>().enabled = false;
            StateMachine = null;
        }
    }

    private void CheckSuperArmor()
    {
        foreach (Material material in materials)
        {
            material.SetFloat("_IsActiveOutline", isSuperArmor ? 1 : 0);
        }
    }

    private void CheckCrowedControl()
    {
        if (isAirborne || isKnockback || isAttack)
        {
            agent.enabled = false;
            GetComponent<CapsuleCollider>().excludeLayers = excludeMaskInAttack;
        }
        else
        {
            if (!agent.enabled)
            {
                agent.enabled = true;

                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }
                else
                {
                    agent.Warp(transform.position);
                }
            }
            
            GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask();
        }
    }    


    public void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(gameObject);
        if (healthBar != null)
        {
            ObjectPoolManager.Instance.Despawn(healthBar.transform.parent.gameObject);
        }
    }

    public override void GetKnockback(Vector3 directionVector, float knockbackForce, float knockbackTime)
    {
        if (isSuperArmor) return;

        base.GetKnockback(directionVector, knockbackForce, knockbackTime);

        animator.Play($"{gameObject.name}Idle");
        animator.SetBool("isMove", false);

        isAttack = false;
        ClearIndicators();
        StateMachine?.ChangeState(ChaseState);
    }

    public override void GetAirBorne(float airborneForce)
    {
        if (isSuperArmor) return;

        base.GetAirBorne(airborneForce);

        animator.Play($"{gameObject.name}Idle");
        animator.SetBool("isMove", false);

        isAttack = false;
        ClearIndicators();
        StateMachine?.ChangeState(ChaseState);
    }

    private void OnDisable()
    {
        isDead = false;
        isKnockback = false;
        isAirborne = false;
        
        knockbackSpan = 0;
        knockbackStartVector = Vector3.zero;
        knockbackVector = Vector3.zero;

        airborneSpan = 0;
        airborneVector = Vector3.zero;
    }

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();

        if (healthBar != null)
        {
            healthBar.transform.parent.transform.position = transform.position + (Vector3.up * 2.5f);
            healthBar.transform.parent.transform.forward = Camera.main.transform.forward;
        }

        CheckDeath();

        if (!isDead)
        {
            attackDelay = Mathf.Max(attackDelay - Time.deltaTime, 0);
            StateMachine?.Update();

            if (boxIndicator != null)
            {
                boxIndicator.transform.position = transform.position + Vector3.up * 0.1f;
            }
            if (roundIndicator != null)
            {
                roundIndicator.transform.position = transform.position + Vector3.up * 0.1f;
            }

            CheckCrowedControl();
            CheckSuperArmor();
        }
        else
        {
            ClearIndicators();

            dissapearAmount = Mathf.Min(dissapearAmount + Time.deltaTime, 1);
            foreach (Material material in materials)
            {
                material.SetFloat("_Dissolve", dissapearAmount);
            }

            if (dissapearAmount >= 1)
            {
                Despawn();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
