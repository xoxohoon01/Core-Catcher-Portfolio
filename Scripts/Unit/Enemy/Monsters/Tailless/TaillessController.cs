using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaillessController : MonsterController
{
    public override string AttackAnimationName => "TaillessAttack";
    public bool isMoving = false;

    public override void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();

        ChaseState = new TaillessChaseState<TaillessController>(this);
        AttackState = new MonsterAttackState<TaillessController>(this);

        StateMachine.ChangeState(ChaseState);
    }

    public void AttackInitialize()
    {
        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position + (transform.forward * 2f);
        Vector3 forwardPos = startRotation * Vector3.forward;
        HitController obj1 = ObjectPoolManager.Instance.Spawn("TaillessAttack", startPos, startRotation).GetComponent<HitController>();
        //obj1.Initialize(status.damage, 0f, 0.2f, 0f, 1f, 0f, this, Faction.Monster, new Vector3(1, 1, 1));
        obj1.InitializeRound(status.damage, 0f, 0.2f, 0f, 01f, 0f, this, Faction.Monster, new Vector3(9, 1, 9), transform.position, forwardPos, 1f, 0.3f, 80f);
        ObjectPoolManager.Instance.Spawn("AudioObject", obj1.transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("GhoulAttack", "MonsterWeapon");
    }
}
