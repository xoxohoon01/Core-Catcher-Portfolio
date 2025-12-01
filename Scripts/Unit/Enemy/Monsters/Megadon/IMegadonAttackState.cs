using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

public class IMegadonAttackState : IMonsterState
{
    private MegadonController monster;
    private Vector3 targetVector;
    private bool isAttackStart;

    public IMegadonAttackState(MegadonController monster)
    {
        this.monster = monster;
    }

    public void OnEnter()
    {
        isAttackStart = false;
    }

    public void OnUpdate()
    {
        if (isAttackStart && monster.isAttack)
        {
            monster.moveVector = targetVector * monster.status.moveSpeed;
            monster.GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("Monster", "Player");
        }
        else if (!isAttackStart && !monster.isAttack)
        {
            PlayerController target = PlayerManager.Instance.GetPlayer();
            if (target == null && !target.isDead) return;
            monster.Target = target.transform;

            if (Vector3.Distance(monster.transform.position, target.transform.position) <= monster.attackRange)
            {
                monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, Quaternion.LookRotation(target.transform.position - monster.transform.position), 1);

                if (monster.attackDelay <= 0)
                {
                    monster.animator.Play("MegadonAttack", 0, 0);
                    monster.attackDelay = (1 / monster.status.attackSpeed);
                    targetVector = target.transform.position - monster.transform.position;
                    isAttackStart = true;
                }
            }
            else
            {
                monster.StateMachine.ChangeState(monster.ChaseState);
            }
        }
    }

    public void OnExit() { }

}
