using UnityEngine;

// 실제 로직은 RavenAttackController.CheckHit()과 FlyingBulletController.CheckHit()에 구현되어 있음
// 이 클래스는 CardManager.artifactEffectLevel["Targeting"] 등록 용도로만 사용
public class Targeting : IArtifactCardEffect
{
    public void ApplyEffect(ArtifactCardScriptableObject card) { }
    public void Update() { }
}
