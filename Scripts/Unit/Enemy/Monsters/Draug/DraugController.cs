using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DraugController : MonsterController
{
    public override void InitializeStateMachine()
    {
        StateMachine = new MonsterStateMachine();
        ChaseState = new IDraugChaseState(this);
        AttackState = new IDraugAttackState(this);

        StateMachine.ChangeState(ChaseState);
    }

    public IEnumerator AttackInitialize()
    {
        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position;
        Vector3 forwardPos = startRotation * Vector3.forward;

        HitController obj1 = ObjectPoolManager.Instance.Spawn("DraugAttack", startPos + (forwardPos * 7f), startRotation).GetComponent<HitController>();
        obj1.Initialize(status.damage, 0, 0.2f, 0f, 1, 0, this, Faction.Monster, new Vector3(4, 4, 4));
        ObjectPoolManager.Instance.Spawn("AudioObject", obj1.transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("DraugAttack", "MonsterWeapon");

        yield return new WaitForSeconds(0.15f);

        HitController obj2 = ObjectPoolManager.Instance.Spawn("DraugAttack", startPos + (forwardPos * 14f), startRotation).GetComponent<HitController>();
        obj2.Initialize(status.damage, 0, 0.2f, 0f, 1, 0, this, Faction.Monster, new Vector3(4, 4, 4));
        ObjectPoolManager.Instance.Spawn("AudioObject", obj1.transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("DraugAttack", "MonsterWeapon");

        yield return new WaitForSeconds(0.15f);

        HitController obj3 = ObjectPoolManager.Instance.Spawn("DraugAttack", startPos + (forwardPos * 21f), startRotation).GetComponent<HitController>();
        obj3.Initialize(status.damage, 0, 0.2f, 0f, 1, 0, this, Faction.Monster, new Vector3(4, 4, 4));
        ObjectPoolManager.Instance.Spawn("AudioObject", obj1.transform.position, Quaternion.identity).GetComponent<AudioObject>().PlayAudio("DraugAttack", "MonsterWeapon");
    }
}
