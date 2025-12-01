using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DecoyBombController : HitController
{
    public ParticleSystem particle;
    public GameObject bombObject;

    public override void Initialize(float startDamage, float startSpeed, float targetHitTime, float startDelayTime, float startLifeTime, float multiHitDelay, Faction senderFaction, Vector3 size)
    {
        base.Initialize(startDamage, startSpeed, targetHitTime, startDelayTime, startLifeTime, multiHitDelay, senderFaction, size);

        var shape = particle.shape;
        shape.radius = size.x * 0.75f;

        var emission = particle.emission;

        // 첫 번째 Burst 가져오기
        Burst burst = emission.GetBurst(0);

        // radius에 따라 Count 값을 동적으로 변경
        float minCount = size.x * 10f;     // 예시: 반지름 * 5
        float maxCount = size.x * 12f;    // 예시: 반지름 * 10
        burst.count = new MinMaxCurve(minCount, maxCount);

        emission.SetBurst(0, burst);
        Invoke("DisableBombObject", startDelayTime);
    }

    public void DisableBombObject()
    {
        bombObject.SetActive(false);
    }
}
