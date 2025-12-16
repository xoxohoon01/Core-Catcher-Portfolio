using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class IMegadonChaseState : IMonsterState
{
    private MegadonController monster;
    private Vector3 dir;
    private NavMeshPath path = new NavMeshPath();

    private float moveStartTime;
    private float totalMoveTime;        // isMoving true 유지 시간

    public IMegadonChaseState(MegadonController monster)
    {
        this.monster = monster;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        PlayerController target = PlayerManager.Instance.GetPlayer();
        if (target == null && !target.isDead) return;

        monster.Target = target.transform;

        float dist = Vector3.Distance(monster.transform.position, target.transform.position);

        if (dist > monster.attackRange)
        {
            monster.animator.SetBool("isMove", true);

            // NavMesh 경로 계산
            if (NavMesh.CalculatePath(
                monster.transform.position,
                target.transform.position,
                NavMesh.AllAreas,
                path) &&
                path.corners.Length >= 2)
            {
                dir = (path.corners[1] - monster.transform.position).normalized;
            }
            else
            {
                // NavMesh 실패 시 직선 방향 fallback
                dir = (target.transform.position - monster.transform.position).normalized;
            }

            //monster.moveVector = (target.transform.position - monster.transform.position).normalized * monster.status.moveSpeed;
            monster.moveVector = dir * monster.status.moveSpeed;

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
