using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

public class MegadonAttackState<T> : MonsterAttackState<MegadonController>
{
    private Vector3 targetVector;
    private bool isAttackStart;

    public MegadonAttackState(MegadonController monster) : base(monster) { }

    public override void OnEnter()
    {
        base.OnEnter();
        isAttackStart = false;
    }

    public override void OnUpdate()
    {
        if (isAttackStart && monster.isAttack)
        {
            monster.moveVector = targetVector * 25f;
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
                    monster.animator.Play(monster.AttackAnimationName, 0, 0);
                    monster.attackDelay = (1 / monster.status.attackSpeed);
                    Vector3 targetPos = target.transform.position;
                    targetPos.y = 0;
                    Vector3 nowPos = monster.transform.position;
                    nowPos.y = 0;
                    targetVector = (targetPos - nowPos).normalized;
                    isAttackStart = true;
                }
            }
            else
            {
                monster.StateMachine.ChangeState(monster.ChaseState);
            }
        }
    }
}
