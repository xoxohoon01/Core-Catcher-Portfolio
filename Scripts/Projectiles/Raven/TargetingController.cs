using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingController : HitController
{
    public ParticleSystem appearVFX1;
    public ParticleSystem appearVFX2;
    public ParticleSystem appearVFX3;
    public ParticleSystem appearVFX4;
    public ParticleSystem coreVFX;

    public override void Initialize(float startDamage, float startSpeed, float targetHitTime, float startDelayTime, float startLifeTime, float multiHitDelay, UnitController sender, Faction senderFaction, Vector3 size)
    {
        base.Initialize(startDamage, startSpeed, targetHitTime, startDelayTime, startLifeTime, multiHitDelay, sender, senderFaction, size);

        appearVFX1.Stop();
        appearVFX2.Stop();
        appearVFX3.Stop();
        appearVFX4.Stop();
        coreVFX.Stop();

        var appear1 = appearVFX1.main;
        var appear2 = appearVFX2.main;
        var appear3 = appearVFX3.main;
        var appear4 = appearVFX4.main;
        appear1.startLifetime = targetHitTime + startDelayTime;
        appear2.startLifetime = targetHitTime + startDelayTime;
        appear3.startLifetime = targetHitTime + startDelayTime;
        appear4.startLifetime = targetHitTime + startDelayTime;

        var vfx = coreVFX.main;
        vfx.duration = targetHitTime;

        appearVFX1.Play();
        appearVFX2.Play();
        appearVFX3.Play();
        appearVFX4.Play();
        coreVFX.Play();
    }
}
