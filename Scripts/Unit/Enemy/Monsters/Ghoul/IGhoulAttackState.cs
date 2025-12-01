using UnityEngine;
using System.Linq;

public class IGhoulAttackState : IMonsterState
{
    private GhoulController monster;

    public IGhoulAttackState(GhoulController monster)
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
            monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, Quaternion.LookRotation(target.transform.position - monster.transform.position), 1);

            if (monster.attackDelay <= 0)
            {
                monster.animator.Play("GhoulAttack", 0, 0);
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
