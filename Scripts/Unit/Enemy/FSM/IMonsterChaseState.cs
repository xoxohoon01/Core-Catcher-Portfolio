using UnityEngine;

public class IMonsterChaseState : IMonsterState
{
    private MonsterController monster;

    public IMonsterChaseState(MonsterController monster)
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

        if (Vector3.Distance(monster.transform.position, target.transform.position) > monster.attackRange)
        {
            monster.animator.SetBool("isMove", true);
            monster.moveVector = (target.transform.position - monster.transform.position).normalized * monster.status.moveSpeed;

            monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, Quaternion.LookRotation(monster.moveVector), Time.deltaTime * 20f);
        }
        else
        {
            monster.animator.SetBool("isMove", false);
            monster.moveVector = Vector3.zero;
            if (monster.attackDelay <= 0)
            {
                monster.attackDelay = (1 / monster.status.attackSpeed);
            }
        }
    }

    public void OnExit() { }
}
