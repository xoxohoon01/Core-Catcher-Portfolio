using UnityEngine;

public class IJawlerChaseState : IMonsterState
{
    private JawlerController monster;

    public IJawlerChaseState(JawlerController monster)
    {
        this.monster = monster;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        //monster.MoveToTarget();

        PlayerController target = PlayerManager.Instance.GetPlayer();
        if (target == null && !target.isDead) return;
        monster.Target = target.transform;

        if (Vector3.Distance(monster.transform.position, target.transform.position) > monster.attackRange)
        {
            monster.animator.SetBool("isMove", true);
            monster.moveVector = (target.transform.position - monster.transform.position).normalized * monster.status.moveSpeed;

            Quaternion targetQuaternion = Quaternion.Lerp(monster.transform.rotation, Quaternion.LookRotation(monster.moveVector), Time.deltaTime * 20f);
            Vector3 euler = targetQuaternion.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            monster.transform.rotation = Quaternion.Euler(euler);
        }
        else
        {
            monster.animator.SetBool("isMove", false);
            monster.moveVector = Vector3.zero;
            
            monster.StateMachine.ChangeState(monster.AttackState);
        }
    }

    public void OnExit() { }
}
