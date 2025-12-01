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
        if (target == null && !target.isDead) return;
        monster.Target = target.transform;

        if (Vector3.Distance(monster.transform.position, target.transform.position) <= monster.attackRange)
        {
            monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, Quaternion.LookRotation(target.transform.position - monster.transform.position), Time.deltaTime * 20f);

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
