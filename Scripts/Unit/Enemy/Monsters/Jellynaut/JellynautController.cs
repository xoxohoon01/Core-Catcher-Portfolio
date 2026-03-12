using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class JellynautController : MonsterController
{
    public override string AttackAnimationName => "JellynautAttack";
    public override void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();
        ChaseState = new MonsterChaseState<JellynautController>(this);
        AttackState = new MonsterAttackState<JellynautController>(this);

        StateMachine.ChangeState(ChaseState);
    }

    public void AttackInitialize()
    {
        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position;
        Vector3 forwardPos = startRotation * Vector3.forward;

        HitController obj = ObjectPoolManager.Instance.Spawn("JellynautAttack", startPos, startRotation).GetComponent<HitController>();
        obj.Initialize(status.damage, 0, 3, 0, 3, 0.5f, this, Faction.Monster, Vector3.one);
        ObjectPoolManager.Instance.Spawn("AudioObject", obj.transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("JawlerAttack", "MonsterWeapon");
    }
}
