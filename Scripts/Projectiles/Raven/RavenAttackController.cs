using UnityEngine;

public class RavenAttackController : BulletController
{
    protected override void CheckHit(UnitController target)
    {
        base.CheckHit(target);
        // FlyingBullet · Targeting 발동은 OnBulletHit 이벤트(FlyingBullet.cs, Targeting.cs)가 처리
        ObjectPoolManager.Instance.Spawn($"AudioObject", transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio($"Hit{Random.Range(1, 3)}", "Hit");
    }
}
