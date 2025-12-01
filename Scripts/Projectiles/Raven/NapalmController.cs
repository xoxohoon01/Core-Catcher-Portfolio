using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NapalmController : HitController
{
    public RFX4_ShaderFloatCurve particle1;
    public RFX4_ShaderFloatCurve particle2;
    public override void Initialize(float startDamage, float startSpeed, float targetHitTime, float startDelayTime, float startLifeTime, float multiHitDelay, Faction senderFaction, Vector3 size)
    {
        base.Initialize(startDamage, startSpeed, targetHitTime, startDelayTime, startLifeTime, multiHitDelay, senderFaction, size);

        particle1.GraphTimeMultiplier = startLifeTime;
        particle2.GraphTimeMultiplier = startLifeTime;

        //Keyframe[] particle1Keys = particle1.FloatCurve.keys;
        //int particle1Last = particle1Keys.Length - 1;
        //particle1Keys[particle1Last].time = startLifeTime;
        //particle1.FloatCurve.keys = particle1Keys;

        //Keyframe[] particle2Keys = particle2.FloatCurve.keys;
        //int particle2Last = particle2Keys.Length - 1;
        //particle2Keys[particle1Last].time = startLifeTime;
        //particle2.FloatCurve.keys = particle2Keys;
    }
}
