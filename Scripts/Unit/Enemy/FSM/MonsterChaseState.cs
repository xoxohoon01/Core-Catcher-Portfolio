using UnityEngine;

public class MonsterChaseState<T> : MonsterStateBase<T> where T : MonsterController
{
    public MonsterChaseState(T monster) : base(monster) { }

    public override void OnUpdate()
    {
        PlayerController target = PlayerManager.Instance.GetPlayer();
        if (target == null || target.isDead) return;
        if (monster.isKnockback || monster.isAirborne) return;

        float distance = Vector3.Distance(monster.transform.position, target.transform.position);

        if (distance > monster.attackRange)
        {
            monster.animator.SetBool("isMove", true);
            monster.moveVector = (target.transform.position - monster.transform.position).normalized * monster.status.moveSpeed;
            monster.transform.rotation = Quaternion.Lerp(monster.transform.rotation, Quaternion.LookRotation(monster.moveVector), Time.deltaTime * 20f);
        }
        else
        {
            monster.animator.SetBool("isMove", false);
            monster.moveVector = Vector3.zero;
            if (!monster.isKnockback && !monster.isAirborne)
            {
                monster.StateMachine.ChangeState(monster.AttackState);
            }
        }
    }
}