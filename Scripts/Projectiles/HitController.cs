using DamageNumbersPro;
using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitController : MonoBehaviour
{
    protected DamageNumber damageNumberPrefab;
    public GameObject particleObject;

    protected bool isInitialized;
    protected new Rigidbody rigidbody;

    protected float damage;
    protected float moveSpeed;
    protected float hitTime;
    protected float startDelay;
    protected float lifeTime;
    protected float currentLifeTime;
    protected float multiHitDelay;
    protected UnitController sender;
    protected Faction faction;

    protected Dictionary<UnitController, HitInfo> hitInfoMap = new Dictionary<UnitController, HitInfo>();

    protected class HitInfo
    {
        public int hitCount = 0; // 피격된 횟수
        public bool canHit = true; // 타격 가능 여부
    }

    public virtual void Initialize(float baseDamage, float moveSpeed, float hitTime, float startDelay, float lifeTime, float multiHitDelay, UnitController sender, Faction senderFaction, Vector3 size)
    {
        this.sender = sender;

        this.damage = baseDamage;
        this.moveSpeed = moveSpeed;
        this.hitTime = hitTime;
        this.startDelay = startDelay;
        this.lifeTime = lifeTime;
        this.multiHitDelay = multiHitDelay;
        faction = senderFaction;

        transform.localScale = size;
        if (particleObject != null)
        {
            particleObject.transform.SetParent(null);
            ParticleSystem[] allParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in allParticles)
            {
                ps.Stop();
            }
        }
    }

    protected virtual void CheckHit(UnitController target)
    {
        CritResult critResult = CriticalCalculator.Calculate(sender);

        float finalDamage = damage * critResult.multiplier;
        float remainDamage = finalDamage;

        if (target.status.shield > 0)
        {
            if (remainDamage <= target.status.shield)
            {
                target.status.shield -= remainDamage;
                DamageNumberManager.Instance.shieldHitPrefab.Spawn(target.transform.position, remainDamage);
                remainDamage = 0;
            }
            else
            {
                float shieldDamage = target.status.shield;

                target.status.shield = 0;

                DamageNumberManager.Instance.shieldHitPrefab
                    .Spawn(target.transform.position, shieldDamage);

                remainDamage -= shieldDamage;
            }
        }
        if (remainDamage > 0)
        {
            target.status.hp -= remainDamage;
            critResult.damageNumber.Spawn(target.transform.position, remainDamage);
        }
    }

    protected virtual void AfterHit(UnitController target)
    {
        if (target.healthBar != null)
        {
            target.healthBar.UpdateBar(target.status.hp, false, UpdateAnim.Damage);
        }
    }

    public int CheckCritical(UnitController sender)
    {
        float crit = sender.status.critChance * 100;

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

    private IEnumerator CoRestoreMultiHit(UnitController target, float time)
    {
        yield return new WaitForSeconds(time);
        hitInfoMap[target].canHit = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isInitialized || (hitTime != 0 && currentLifeTime > hitTime) || startDelay > 0) return;

        if (other.GetComponent<UnitController>() != null)
        {
            UnitController target = other.GetComponent<UnitController>();
            if (target.faction != faction)
            {
                if (target.faction == Faction.Player)
                {
                    if (target.GetComponent<PlayerController>().dashSpan > 0)
                        return;
                }
                if (!hitInfoMap.ContainsKey(target))
                {
                    HitInfo newHitInfo = new HitInfo();
                    newHitInfo.canHit = false;
                    hitInfoMap.Add(target, newHitInfo);
                    CheckHit(target);
                    AfterHit(target);
                    if (multiHitDelay != 0)
                        StartCoroutine(CoRestoreMultiHit(target, multiHitDelay));
                }
                else if (hitInfoMap[target].canHit)
                {
                    hitInfoMap[target].canHit = false;
                    CheckHit(target);
                    AfterHit(target);
                    if (multiHitDelay != 0)
                        StartCoroutine(CoRestoreMultiHit(target, multiHitDelay));
                }
            }
        }
    }

    private void OnDisable()
    {
        currentLifeTime = 0;
        hitInfoMap.Clear();
        isInitialized = false;
    }

    protected virtual void Update()
    {
        if (BattleManager.Instance.isStop) return;

        if (startDelay > 0)
        {
            startDelay -= Time.deltaTime;
        }

        if (startDelay <= 0)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                if (particleObject != null)
                {
                    ParticleSystem[] allParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in allParticles)
                    {
                        ps.Play();
                    }
                }
            }

            if (currentLifeTime >= lifeTime)
            {
                if (particleObject != null)
                {
                    ParticleSystem[] allParticles = particleObject.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in allParticles)
                    {
                        ps.Stop();
                    }
                }
                ObjectPoolManager.Instance.Despawn(gameObject);
            }

            currentLifeTime += Time.deltaTime;
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}
