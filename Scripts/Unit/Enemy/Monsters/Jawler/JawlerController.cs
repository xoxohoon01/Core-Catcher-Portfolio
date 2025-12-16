using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class JawlerController : MonsterController
{
    public Image attackIndicator;

    public override void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();
        ChaseState = new IJawlerChaseState(this);
        AttackState = new IJawlerAttackState(this);

        StateMachine.ChangeState(ChaseState);
        attackIndicator.gameObject.SetActive(false);
    }

    public void AttackInitialize()
    {
        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position + Vector3.up;
        Vector3 forwardPos = startRotation * Vector3.forward;

        BulletController obj = ObjectPoolManager.Instance.Spawn("JawlerAttack", startPos, startRotation).GetComponent<BulletController>();
        obj.Initialize(status.damage, 1, 30, 5, false, Faction.Monster, Vector3.one * 2);
        ObjectPoolManager.Instance.Spawn("AudioObject", obj.transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("JawlerAttack", "MonsterWeapon");
    }
}
