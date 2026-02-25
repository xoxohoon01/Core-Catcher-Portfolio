using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController : UnitController
{
    public CharacterScriptableObject characterData;
    public GameObject shield;

    private bool isDash;
    protected bool isAttack;
    protected bool isSkill;

    private Vector3 dashVector;
    public float dashDelay;
    public float dashSpan { get; private set; }

    protected Vector3 targetVector;

    public float attackDelay { get; protected set; }
    protected float attackSpan;

    protected SkillData currentSkill;
    protected int currentSkillIndex = -1;

    public float[] skillDelay;
    protected float[] skillSpan;

    protected virtual void Initialize()
    {
        PlayerAnimationHashTable.Initialize(animator.runtimeAnimatorController);

        baseStatus.damage = characterData.damage;
        baseStatus.armor = characterData.armor;
        baseStatus.moveSpeed = characterData.moveSpeed;
        baseStatus.attackSpeed = characterData.attackSpeed;
        baseStatus.maxHP = characterData.maxHP;
        baseStatus.cooldownReduction = 0;
        baseStatus.skillRange = 1;
        baseStatus.skillSpeed = 1;
        baseStatus.critChance = 0;
        baseStatus.critDamage = 2;
        baseStatus.skillDamage = 1;
        baseStatus.drain = 0;
        baseStatus.dashCooldown = characterData.dashCooldown;

        isDirty = true;
        RecalculateStats();

        status.hp = status.maxHP;
        status.maxExp = 100;

        skillDelay = new float[characterData.skills.Length];
        skillSpan = new float[characterData.skills.Length];
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
            animator.speed = clip.length / characterData.dashDuration;
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
            DashStart(characterData.dashClipName);
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
            CancelBasicAttack();
            CancelSkill();

            isDash = true;
            animator.SetBool("isDash", true);
            animator.speed = 1f;

            Vector3 direction = ray.GetPoint(distance) - transform.position;
            direction.y = 0;

            dashVector = direction.normalized * characterData.dashForce;
            dashDelay = status.dashCooldown;
            dashSpan = characterData.dashDuration;

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

        if (Input.GetMouseButton(0))
        {
            if (!isDash && !isSkill && attackDelay <= 0)
            {
                // 공격 콤보별 딜레이는 하위 클래스의 BasicAttack() 안에서 계산.
                BasicAttack();
                isAttack = true;
            }
        }

        if (!isSkill && !isDash)
        {
            if (Input.GetKeyDown(KeyCode.Q)) UseSkill(0);
            if (Input.GetKeyDown(KeyCode.E)) UseSkill(1);
            if (Input.GetKeyDown(KeyCode.F)) UseSkill(2);
            if (Input.GetKeyDown(KeyCode.R)) UseSkill(3);
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

            combo = (combo + 1) > characterData.maxCombo - 1 ? 0 : combo + 1;
            animator.SetInteger("attackCombo", combo);

            float delay;
            float span;

            string clipName;

            if (combo == 0)
            {
                clipName = characterData.attack1ClipName;
                delay = (1 / status.attackSpeed) * characterData.attack1Time;
                span = (1 / status.attackSpeed)* characterData.attack1Time;
            }
            else if (combo == 1)
            {
                clipName = characterData.attack2ClipName;
                delay = (1 / status.attackSpeed) * characterData.attack2Time;
                span = (1 / status.attackSpeed) * characterData.attack2Time;
            }
            else if (combo == 2)
            {
                clipName = characterData.attack3ClipName;
                delay = (1 / status.attackSpeed) * characterData.attack3Time;
                span = (1 / status.attackSpeed) * characterData.attack3Time;
            }
            else
            {
                clipName = characterData.attack4ClipName;
                delay = (1 / status.attackSpeed) * characterData.attack4Time;
                span = (1 / status.attackSpeed) * characterData.attack4Time;
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
    protected void CancelBasicAttack()
    {
        if (!isAttack) return;

        isAttack = false;
        attackSpan = 0;
        attackDelay = 0;

        animator.SetBool("isAttack", false);
        animator.SetInteger("attackCombo", -1);
    }

    protected void UseSkill(int index)
    {
        if (index < 0 || index >= characterData.skills.Length)
            return;

        if (skillDelay[index] > 0)
            return;

        if (isDash)
            return;

        if (isAttack)
            CancelBasicAttack();

        SkillData skill = characterData.skills[index];

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        if (!plane.Raycast(ray, out float distance))
            return;

        isSkill = true;
        currentSkill = skill;
        currentSkillIndex = index;

        moveVector = Vector3.zero;

        targetVector = ray.GetPoint(distance) - transform.position;
        targetVector.y = 0;

        transform.rotation = Quaternion.LookRotation(targetVector);

        animator.SetBool("isSkill", true);

        float finalSpan =
            (skill.baseSpan / (1 + ((status.attackSpeed / baseStatus.attackSpeed) * 0.1f)))
            / status.skillSpeed;

        float finalCooldown =
            skill.baseCooldown * (1 - status.cooldownReduction);

        skillSpan[index] = finalSpan;
        skillDelay[index] = finalCooldown;

        animator.Play(skill.animationClipName);

        var clip = animator.runtimeAnimatorController
               .animationClips
               .FirstOrDefault(c => c.name == skill.animationClipName);

        if (clip != null)
        {
            animator.speed = clip.length / finalSpan;
        }

        skill.OnSkillStart(this);
    }
    public void OnSkillAnimationEvent(int number)
    {
        if (!isSkill) return;
        if (currentSkill == null) return;

        currentSkill.OnAnimationEvent(this, number);
    }
    protected void EndSkill()
    {
        if (currentSkill != null)
        {
            currentSkill.OnSkillEnd(this);
        }

        currentSkill = null;
        currentSkillIndex = -1;
        isSkill = false;
        animator.SetBool("isSkill", false);
    }
    protected void CancelSkill()
    {
        if (!isSkill) return;

        if (currentSkill != null)
        {
            currentSkill.OnSkillCancel(this);
        }

        if (currentSkillIndex >= 0)
        {
            skillSpan[currentSkillIndex] = 0;
        }

        currentSkill = null;
        currentSkillIndex = -1;
        isSkill = false;

        animator.SetBool("isSkill", false);
    }

    protected virtual void Skill1()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Plane plane = new Plane(Vector3.up, Vector3.up);

        //if (plane.Raycast(ray, out float distance))
        //{
        //    isSkill = true;
        //    moveVector = Vector3.zero;

        //    targetVector = ray.GetPoint(distance) - transform.position;
        //    targetVector.y = 0;

        //    transform.rotation = Quaternion.LookRotation(targetVector);

        //    animator.SetBool("isSkill", true);
        //    skill1Span = (characterData.skill1Span / (1 + ((status.attackSpeed / characterData.attackSpeed) * 0.1f))) / status.skillSpeed;
        //    skill1Delay = characterData.skill1Cooldown * (1 - status.cooldownReduction);

        //    int stateHash = Animator.StringToHash(characterData.skill1ClipName);
        //    animator.Play(stateHash);
        //    var clip = animator.runtimeAnimatorController
        //           .animationClips
        //           .FirstOrDefault(c => c.name == characterData.skill1ClipName);
        //    if (clip != null)
        //    {
        //        animator.speed = clip.length / (skill1Span);
        //    }
        //}
    }
    protected virtual void Skill2()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Plane plane = new Plane(Vector3.up, Vector3.up);

        //if (plane.Raycast(ray, out float distance))
        //{
        //    isSkill = true;
        //    moveVector = Vector3.zero;

        //    targetVector = ray.GetPoint(distance) - transform.position;
        //    targetVector.y = 0;

        //    transform.rotation = Quaternion.LookRotation(targetVector);

        //    animator.SetBool("isSkill", true);
        //    skill2Span = (characterData.skill2Span / (1 + ((status.attackSpeed / characterData.attackSpeed) * 0.1f))) / status.skillSpeed;
        //    skill2Delay = characterData.skill2Cooldown * (1 - status.cooldownReduction);

        //    int stateHash = Animator.StringToHash(characterData.skill2ClipName);
        //    animator.Play(stateHash);
        //    var clip = animator.runtimeAnimatorController
        //           .animationClips
        //           .FirstOrDefault(c => c.name == characterData.skill2ClipName);
        //    if (clip != null)
        //    {
        //        animator.speed = clip.length / (skill2Span);
        //    }
        //}
    }
    protected virtual void Skill3()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Plane plane = new Plane(Vector3.up, Vector3.up);

        //if (plane.Raycast(ray, out float distance))
        //{
        //    isSkill = true;
        //    moveVector = Vector3.zero;

        //    targetVector = ray.GetPoint(distance) - transform.position;
        //    targetVector.y = 0;

        //    transform.rotation = Quaternion.LookRotation(targetVector);

        //    animator.SetBool("isSkill", true);
        //    skill3Span = (characterData.skill3Span / (1 + ((status.attackSpeed / characterData.attackSpeed) * 0.1f))) / status.skillSpeed;
        //    skill3Delay = characterData.skill3Cooldown * (1 - status.cooldownReduction);

        //    int stateHash = Animator.StringToHash(characterData.skill3ClipName);
        //    animator.Play(stateHash);
        //    var clip = animator.runtimeAnimatorController
        //           .animationClips
        //           .FirstOrDefault(c => c.name == characterData.skill3ClipName);
        //    if (clip != null)
        //    {
        //        animator.speed = clip.length / (skill3Span);
        //    }
        //}
    }
    protected virtual void Skill4()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Plane plane = new Plane(Vector3.up, Vector3.up);

        //if (plane.Raycast(ray, out float distance))
        //{
        //    isSkill = true;
        //    moveVector = Vector3.zero;

        //    targetVector = ray.GetPoint(distance) - transform.position;
        //    targetVector.y = 0;

        //    transform.rotation = Quaternion.LookRotation(targetVector);

        //    animator.SetBool("isSkill", true);
        //    skill4Span = (characterData.skill4Span / (1 + ((status.attackSpeed / characterData.attackSpeed) * 0.1f))) / status.skillSpeed;
        //    skill4Delay = characterData.skill4Cooldown * (1 - status.cooldownReduction);

        //    int stateHash = Animator.StringToHash(characterData.skill4ClipName);
        //    animator.Play(stateHash);
        //    var clip = animator.runtimeAnimatorController
        //           .animationClips
        //           .FirstOrDefault(c => c.name == characterData.skill4ClipName);
        //    if (clip != null)
        //    {
        //        animator.speed = clip.length / (skill4Span);
        //    }
        //}
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

            animator.Play($"{characterData.characterName}Death");
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

        GameManager.Instance.AddCharacterObject(gameObject);
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

            // 쿨타임 계산
            for (int i = 0; i < characterData.skills.Length; i++)
            {
                if (skillSpan[i] <= 0)
                    skillDelay[i] = Mathf.Max(skillDelay[i] - Time.deltaTime, 0);

                skillSpan[i] = Mathf.Max(skillSpan[i] - Time.deltaTime, 0);
                if (isSkill && currentSkillIndex >= 0)
                {
                    if (skillSpan[currentSkillIndex] <= 0)
                    {
                        EndSkill();
                    }
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        shield.transform.position = transform.position;
    }
}
