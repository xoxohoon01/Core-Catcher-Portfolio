using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MegadonController : MonsterController
{
    public bool isMoving = false;

    public override void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();
        ChaseState = new IMegadonChaseState(this);
        AttackState = new IMegadonAttackState(this);

        StateMachine.ChangeState(ChaseState);
    }

    public void AttackInitialize()
    {
        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position;
        Vector3 forwardPos = startRotation * Vector3.forward;

        isAttack = true;
        var clip = animator.runtimeAnimatorController
                   .animationClips
                   .FirstOrDefault(c => c.name == "Glide");
        attackDelay = (1 / status.attackSpeed) + clip.length;

        HitController obj = ObjectPoolManager.Instance.Spawn("MegadonAttack", startPos, startRotation).GetComponent<HitController>();
        obj.Initialize(status.damage, 0, 0, 0, 0.5f, 0.5f, this, Faction.Monster, Vector3.one);
        obj.transform.SetParent(transform);
    }
}
