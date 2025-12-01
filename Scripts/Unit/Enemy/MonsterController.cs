using Microlight.MicroBar;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;

public enum MonsterState { Chase, Attack, Die }

public class MonsterController : UnitController
{
    [Header("References")]
    [HideInInspector] public Transform Target; // 추적 대상 (Player)

    [Header("FSM")]
    [HideInInspector] public MonsterStateMachine StateMachine;
    [HideInInspector] public IMonsterState ChaseState;
    [HideInInspector] public IMonsterState AttackState;

    public LayerMask excludeMaskInAttack;

    public bool isAttack;
    public float attackDelay;
    public float attackRange;

    public void Initialize()
    {
        MonsterAppearance appearance = ObjectPoolManager.Instance.Spawn("MonsterAppearance", transform.position, Quaternion.identity).GetComponent<MonsterAppearance>();
        appearance.Initialize(3f);
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        appearance.transform.localScale = new Vector3(collider.bounds.size.x, collider.bounds.size.y, collider.bounds.size.z);

        Status originalStatus = Resources.Load($"Enemies/{gameObject.name}").GetComponent<MonsterController>().status;

        // 능력치 설정
        status.exp = originalStatus.exp;
        status.hp = originalStatus.hp;
        status.armor = originalStatus.armor;
        status.damage = originalStatus.damage;
        status.moveSpeed = originalStatus.moveSpeed;
        status.attackSpeed = originalStatus.attackSpeed;

        // 상태 설정
        isAttack = false;

        InitializeStateMachine();
        healthBar = ObjectPoolManager.Instance.Spawn("HealthBar", transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<MicroBar>();
        healthBar.Initialize(originalStatus.hp);
    }

    public virtual void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();
        ChaseState = new IMonsterChaseState(this);
        AttackState = new IMonsterAttackState(this);

        StateMachine.ChangeState(ChaseState);
    }

    public GameObject DetectPlayer()
    {
        return null;
    }

    public void CheckDeath()
    {
        if (!isDead && status.hp <= 0)
        {
            isDead = true;

            PlayerManager.Instance.GetExp(status.exp);
            ObjectPoolManager.Instance.Despawn(gameObject);
            if (healthBar != null)
            {
                ObjectPoolManager.Instance.Despawn(healthBar.transform.parent.gameObject);
            }
        }
    }

    public void Attack(PlayerController player)
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            if (player.dashSpan <= 0)
            {
                player.status.hp -= status.damage;
            }
        }
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

        attackDelay = Mathf.Max(attackDelay - Time.deltaTime, 0);
        StateMachine.Update();
        //MoveToTarget();

        if (isAirborne || isKnockback || isAttack)
        {
            GetComponent<CapsuleCollider>().excludeLayers = excludeMaskInAttack;
        }
        else
        {
            GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
