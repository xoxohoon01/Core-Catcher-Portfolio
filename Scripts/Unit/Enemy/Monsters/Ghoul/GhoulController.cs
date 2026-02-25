using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GhoulController : MonsterController
{
    public override void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();
        ChaseState = new IGhoulChaseState(this);
        AttackState = new IGhoulAttackState(this);

        StateMachine.ChangeState(ChaseState);
    }

    public void AttackInitialize()
    {
        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position;
        Vector3 forwardPos = startRotation * Vector3.forward;
        HitController obj1 = ObjectPoolManager.Instance.Spawn("GhoulAttack", startPos, startRotation).GetComponent<HitController>();
        obj1.Initialize(status.damage, 0f, 0.2f, 0f, 1f, 0f, this, Faction.Monster, new Vector3(2, 2, 2));
        ObjectPoolManager.Instance.Spawn("AudioObject", obj1.transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("GhoulAttack", "MonsterWeapon");
    }
}
