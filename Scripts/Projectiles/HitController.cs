using DamageNumbersPro;
using Microlight.MicroBar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    public static Action OnPlayerHit;

    protected DamageNumber damageNumberPrefab;
    public GameObject particleObject;
    public GameObject audioObject;

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
        if (audioObject != null)
        {
            AudioSource[] allAudios = audioObject.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource audio in allAudios)
            {
                audio.Pause();
            }
        }
    }

    protected virtual void CheckHit(UnitController target)
    {
        CritResult critResult = CriticalCalculator.Calculate(sender);

        float finalDamage = damage * critResult.multiplier;

        // 방어력 계산
        float armor = target.status.armor;
        float damageReduction = armor / (armor + 300f);
        finalDamage = finalDamage * (1f - damageReduction);

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

        OnPlayerHit?.Invoke();
    }

    protected virtual void AfterHit(UnitController target)
    {
        if (target.healthBar != null)
        {
            target.healthBar.UpdateBar(target.status.hp, false, UpdateAnim.Damage);
        }
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
                if (audioObject != null)
                {
                    AudioSource[] allAudios = audioObject.GetComponentsInChildren<AudioSource>();
                    foreach (AudioSource audio in allAudios)
                    {
                        audio.Play();
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
                if (audioObject != null)
                {
                    AudioSource[] allAudios = audioObject.GetComponentsInChildren<AudioSource>();
                    foreach (AudioSource audio in allAudios)
                    {
                        audio.Stop();
                    }
                }
                ObjectPoolManager.Instance.Despawn(gameObject);
            }

            currentLifeTime += Time.deltaTime;
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}
