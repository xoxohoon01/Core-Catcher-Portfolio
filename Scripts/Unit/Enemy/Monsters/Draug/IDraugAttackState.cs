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
                if (Random.Range(1f, 100f) <= 70)
                {
                    monster.animator.Play("DraugHandAttack", 0, 0);
                }
                else
                {
                    monster.animator.Play("DraugSmash", 0, 0);
                }
                    
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
