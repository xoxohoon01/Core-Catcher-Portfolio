using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController : UnitController
{
    public CharacterScriptableObject character;
    public GameObject shield;

    private bool isDash;
    protected bool isAttack;
    protected bool isSkill;

    private Vector3 dashVector;
    public float dashDelay { get; protected set; }
    public float dashSpan { get; private set; }

    protected Vector3 targetVector;

    public float attackDelay { get; protected set; }
    protected float attackSpan;

    public float skill1Delay { get; protected set; }
    protected float skill1Span;

    public float skill2Delay { get; protected set; }
    protected float skill2Span;

    public float skill3Delay { get; protected set; }
    protected float skill3Span;

    public float skill4Delay { get; protected set; }
    protected float skill4Span;

    protected virtual void Initialize()
    {
        PlayerManager.Instance.SetPlayer(this);

        PlayerAnimationHashTable.Initialize(animator.runtimeAnimatorController);

        status.maxExp = 100;

        status.damage = character.damage;
        status.armor = character.armor;
        status.moveSpeed = character.moveSpeed;
        status.attackSpeed = character.attackSpeed;
        status.maxHP = character.maxHP;
        status.hp = status.maxHP;
        status.decreaseCooldown = 0;
        status.skillRange = 1;
        status.skillSpeed = 1;
    }

    protected void StartAnimation(string animationName)
    {
        int stateHash = Animator.StringToHash(animationName);
        animator.Play(stateHash);
        var clip = animator.runtimeAnimatorController
               .animationClips
               .FirstOrDefault(c => c.name == animationName);
        if (clip != null)
        {
            animator.speed = clip.length / character.dashDuration;
        }
    }

    private void Move()
    {
        bool isGrounded = IsGrounded();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetBool("isMove", true);

            if (isDash)
            {
                moveVector = dashVector;
                return;
            }
            if (isAttack || isSkill)
            {
                return;
            }

            moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * status.moveSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * 20f);

            animator.speed = 1 / (3 / status.moveSpeed);
        }
        else
        {
            animator.SetBool("isMove", false);

            if (isDash)
            {
                moveVector = dashVector;
                return;
            }
            if (isAttack || isSkill)
            {
                return;
            }

            moveVector = new Vector3(0, 0, 0);

            animator.speed = 1;
        }

    }

    private void Dash()
    {
        dashSpan = Mathf.Max(dashSpan - Time.deltaTime, 0);

        if (dashSpan <= 0)
        {
            GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask();
            dashDelay = Mathf.Max(dashDelay - Time.deltaTime, 0);
            dashVector = Vector3.zero;
            isDash = false;
            animator.SetBool("isDash", false);
        }

        if (Input.GetButtonDown("Dash") && dashDelay <= 0)
        {
            DashStart(character.dashClipName);
        }
    }
    
    protected virtual void DashInitialize()
    {

    }

    protected void DashStart(string dashAnimationName)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (plane.Raycast(ray, out float distance))
        {
            isDash = true;
            isAttack = false;
            isSkill = false;
            animator.SetBool("isDash", true);

            Vector3 direction = ray.GetPoint(distance) - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Quaternion rotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);
            dashVector = direction.normalized * character.dashForce;
            dashDelay = character.dashCooldown;
            dashSpan = character.dashDuration;

            transform.rotation = Quaternion.LookRotation(direction);
            GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("Monster");

            StartAnimation(dashAnimationName);
            DashInitialize();
        }
    }

    private void Attack()
    {
        attackDelay = Mathf.Max(attackDelay - Time.deltaTime, 0);

        attackSpan = Mathf.Max(attackSpan - Time.deltaTime, 0);
        if (skill1Span <= 0) skill1Delay = Mathf.Max(skill1Delay - Time.deltaTime, 0);
        skill1Span = Mathf.Max(skill1Span - Time.deltaTime, 0);

        if (skill2Span <= 0) skill2Delay = Mathf.Max(skill2Delay - Time.deltaTime, 0);
        skill2Span = Mathf.Max(skill2Span - Time.deltaTime, 0);

        if (skill3Span <= 0) skill3Delay = Mathf.Max(skill3Delay - Time.deltaTime, 0);
        skill3Span = Mathf.Max(skill3Span - Time.deltaTime, 0);

        if (skill4Span <= 0) skill4Delay = Mathf.Max(skill4Delay - Time.deltaTime, 0);
        skill4Span = Mathf.Max(skill4Span - Time.deltaTime, 0);

        if (attackDelay <= 0)
        {
            if (!isAttack)
                animator.SetInteger("attackCombo", -1);
        }

        if (attackSpan <= 0)
        {
            isAttack = false;
            animator.SetBool("isAttack", false);
        }

        if (skill1Span <= 0 && skill2Span <= 0 && skill3Span <= 0 && skill4Span <= 0)
        {
            isSkill = false;
            animator.SetBool("isSkill", false);
        }

        if (Input.GetMouseButton(0))
        {
            if (!isDash && !isSkill && attackDelay <= 0)
            {
                // 공격 콤보별 딜레이는 하위 클래스의 BasicAttack() 안에서 계산.
                BasicAttack();
                isAttack = true;
            }
        }

        if (!isSkill)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (skill1Delay <= 0)
                {
                    Skill1();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (skill2Delay <= 0)
                {
                    Skill2();
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (skill3Delay <= 0)
                {
                    Skill3();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (skill4Delay <= 0)
                {
                    Skill4();
                }
            }
        }

    }
    
    protected virtual void BasicAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);


        if (plane.Raycast(ray, out float distance))
        {
            moveVector = Vector3.zero;

            targetVector = ray.GetPoint(distance) - transform.position;
            targetVector.y = 0;

            transform.rotation = Quaternion.LookRotation(targetVector);

            // animator.SetTrigger("attackTrigger");
            animator.SetBool("isAttack", true);
            int combo = animator.GetInteger("attackCombo");

            combo = (combo + 1) > character.maxCombo - 1 ? 0 : combo + 1;
            animator.SetInteger("attackCombo", combo);

            float delay;
            float span;

            string clipName;

            if (combo == 0)
            {
                clipName = character.attack1ClipName;
                delay = (1 / status.attackSpeed) * character.attack1Time;
                span = (1 / status.attackSpeed)* character.attack1Time;
            }
            else if (combo == 1)
            {
                clipName = character.attack2ClipName;
                delay = (1 / status.attackSpeed) * character.attack2Time;
                span = (1 / status.attackSpeed) * character.attack2Time;
            }
            else if (combo == 2)
            {
                clipName = character.attack3ClipName;
                delay = (1 / status.attackSpeed) * character.attack3Time;
                span = (1 / status.attackSpeed) * character.attack3Time;
            }
            else
            {
                clipName = character.attack4ClipName;
                delay = (1 / status.attackSpeed) * character.attack4Time;
                span = (1 / status.attackSpeed) * character.attack4Time;
            }

            attackDelay = delay * 0.95f;
            attackSpan = span;

            int stateHash = Animator.StringToHash(clipName);
            animator.Play(stateHash);
            var clip = animator.runtimeAnimatorController
                   .animationClips
                   .FirstOrDefault(c => c.name == clipName);
            if (clip != null)
            {
                animator.speed = clip.length / span;
            }
        }
    }
    public virtual void BasicAttackInitialize(int number)
    {

    }

    protected virtual void Skill1()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (plane.Raycast(ray, out float distance))
        {
            isSkill = true;
            moveVector = Vector3.zero;

            targetVector = ray.GetPoint(distance) - transform.position;
            targetVector.y = 0;

            transform.rotation = Quaternion.LookRotation(targetVector);

            animator.SetBool("isSkill", true);
            skill1Span = (character.skill1Span / (1 + ((status.attackSpeed / character.attackSpeed) * 0.1f))) / status.skillSpeed;
            skill1Delay = character.skill1Cooldown * (1 - status.decreaseCooldown);

            int stateHash = Animator.StringToHash(character.skill1ClipName);
            animator.Play(stateHash);
            var clip = animator.runtimeAnimatorController
                   .animationClips
                   .FirstOrDefault(c => c.name == character.skill1ClipName);
            if (clip != null)
            {
                animator.speed = clip.length / (skill1Span);
            }
        }
    }
    protected virtual void Skill2()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (plane.Raycast(ray, out float distance))
        {
            isSkill = true;
            moveVector = Vector3.zero;

            targetVector = ray.GetPoint(distance) - transform.position;
            targetVector.y = 0;

            transform.rotation = Quaternion.LookRotation(targetVector);

            animator.SetBool("isSkill", true);
            skill2Span = (character.skill2Span / (1 + ((status.attackSpeed / character.attackSpeed) * 0.1f))) / status.skillSpeed;
            skill2Delay = character.skill2Cooldown * (1 - status.decreaseCooldown);

            int stateHash = Animator.StringToHash(character.skill2ClipName);
            animator.Play(stateHash);
            var clip = animator.runtimeAnimatorController
                   .animationClips
                   .FirstOrDefault(c => c.name == character.skill2ClipName);
            if (clip != null)
            {
                animator.speed = clip.length / (skill2Span);
            }
        }
    }
    protected virtual void Skill3()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (plane.Raycast(ray, out float distance))
        {
            isSkill = true;
            moveVector = Vector3.zero;

            targetVector = ray.GetPoint(distance) - transform.position;
            targetVector.y = 0;

            transform.rotation = Quaternion.LookRotation(targetVector);

            animator.SetBool("isSkill", true);
            skill3Span = (character.skill3Span / (1 + ((status.attackSpeed / character.attackSpeed) * 0.1f))) / status.skillSpeed;
            skill3Delay = character.skill3Cooldown * (1 - status.decreaseCooldown);

            int stateHash = Animator.StringToHash(character.skill3ClipName);
            animator.Play(stateHash);
            var clip = animator.runtimeAnimatorController
                   .animationClips
                   .FirstOrDefault(c => c.name == character.skill3ClipName);
            if (clip != null)
            {
                animator.speed = clip.length / (skill3Span);
            }
        }
    }
    protected virtual void Skill4()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (plane.Raycast(ray, out float distance))
        {
            isSkill = true;
            moveVector = Vector3.zero;

            targetVector = ray.GetPoint(distance) - transform.position;
            targetVector.y = 0;

            transform.rotation = Quaternion.LookRotation(targetVector);

            animator.SetBool("isSkill", true);
            skill4Span = (character.skill4Span / (1 + ((status.attackSpeed / character.attackSpeed) * 0.1f))) / status.skillSpeed;
            skill4Delay = character.skill4Cooldown * (1 - status.decreaseCooldown);

            int stateHash = Animator.StringToHash(character.skill4ClipName);
            animator.Play(stateHash);
            var clip = animator.runtimeAnimatorController
                   .animationClips
                   .FirstOrDefault(c => c.name == character.skill4ClipName);
            if (clip != null)
            {
                animator.speed = clip.length / (skill4Span);
            }
        }
    }

    public virtual void Skill1Initialize(int number)
    {

    }
    public virtual void Skill2Initialize(int number)
    {

    }
    public virtual void Skill3Initialize(int number)
    {

    }
    public virtual void Skill4Initialize(int number)
    {
    }

    public void CheckDeath()
    {
        if (!isDead && status.hp <= 0)
        {
            isDead = true;
            moveVector = Vector3.zero;

            animator.Play($"{character.characterName}Death");
            UIManager.Instance.Show<GameOver>().Initialize();
        }
    }
    public float CheckCritical()
    {
        float crit = status.critChance;

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
            crit -= 100f;
        }

        if (ultraCrit)
            return 4;
        else if (superCrit)
            return 3;
        else if (normalCrit)
            return 2;
        else
            return 1;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        shield.transform.SetParent(null);
        Initialize();
    }

    protected override void Update()
    {
        base.Update();

        if (status.shield > 0)
        {
            shield.SetActive(true);
            shield.transform.position = transform.position;
        }
        else
        {
            shield.SetActive(false);
        }
        
        CheckDeath();

        if (!BattleManager.Instance.isStop && !isDead)
        {
            Move();
            Dash();
            Attack();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        shield.transform.position = transform.position;
    }
}
