using UnityEngine;

// 실제 로직은 RavenAttackController.CheckHit()에 구현되어 있음
// 이 클래스는 CardManager.artifactEffectLevel["FlyingBullet"] 등록 용도로만 사용
public class FlyingBullet : IArtifactCardEffect
{
    public void ApplyEffect(ArtifactCardScriptableObject card) { }
    public void Update() { }
}
