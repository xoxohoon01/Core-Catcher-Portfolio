using UnityEngine;
using System.Linq;

public class IDraugAttackState : IMonsterState
{
    private DraugController monster;

    public IDraugAttackState(DraugController monster)
    {
        this.monster = monster;
    }

    public void OnEnter()
    {
    }

    public void OnUpdate()
    {
        if (monster.isAttack)
        {
            return;
        }

        PlayerController target = PlayerManager.Instance.GetPlayer();
        if (target == null || target.isDead) return;
        monster.Target = target.transform;

        if (Vector3.Distance(monster.transform.position, target.transform.position) <= monster.attackRange)
        {
            // È¸Àü
            Quaternion targetRot = Quaternion.LookRotation(target.transform.position - monster.transform.position);
            Vector3 euler = targetRot.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            monster.transform.rotation = Quaternion.Euler(euler);

            if (monster.attackDelay <= 0)
            {
                monster.animator.Play("DraugAttack", 0, 0);
                monster.isAttack = true;
                monster.attackDelay = (1 / monster.status.attackSpeed);
            }
        }
        else
        {
            monster.StateMachine.ChangeState(monster.ChaseState);
        }
    }

    public void OnExit() { }

}
