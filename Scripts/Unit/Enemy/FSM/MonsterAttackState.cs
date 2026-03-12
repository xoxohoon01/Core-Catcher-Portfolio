using UnityEngine;

public class MonsterAttackState<T> : MonsterStateBase<T> where T : MonsterController
{
    public MonsterAttackState(T monster) : base(monster) { }

    public override void OnUpdate()
    {
        if (monster.isAttack) return; // 이미 공격 애니메이션 재생 중이면 리턴

        PlayerController target = PlayerManager.Instance.GetPlayer();
        if (target == null || target.isDead) return;
        if (monster.isKnockback || monster.isAirborne) return;

        float distance = Vector3.Distance(monster.transform.position, target.transform.position);

        if (distance <= monster.attackRange)
        {
            // 타겟 바라보기
            Vector3 lookDir = (target.transform.position - monster.transform.position).normalized;
            monster.transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));

            if (monster.attackDelay <= 0)
            {
                // 애니메이션 이름은 각 컨트롤러의 프로퍼티에서 가져옴
                monster.animator.Play(monster.AttackAnimationName, 0, 0);
                monster.isAttack = true;
                monster.attackDelay = (1 / monster.status.attackSpeed);
            }
        }
        else
        {
            // 사거리를 벗어나면 다시 추격
            monster.StateMachine.ChangeState(monster.ChaseState);
        }
    }
}
