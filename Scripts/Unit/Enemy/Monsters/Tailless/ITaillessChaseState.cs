using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ITaillessChaseState : IMonsterState
{
    private TaillessController monster;

    private bool hasDirection;
    private Quaternion lastRotation;

    private float moveStartTime;
    private float totalMoveTime;

    private Vector3 dir;
    private NavMeshPath path = new NavMeshPath();

    public ITaillessChaseState(TaillessController monster)
    {
        this.monster = monster;
    }

    public void OnEnter()
    {
        hasDirection = false;
        monster.moveVector = Vector3.zero;
    }

    public void OnUpdate()
    {
        PlayerController target = PlayerManager.Instance.GetPlayer();
        if (target == null || target.isDead) return;

        monster.Target = target.transform;

        float dist = Vector3.Distance(monster.transform.position, target.transform.position);

        if (dist > monster.attackRange)
        {
            monster.animator.SetBool("isMove", true);

            if (monster.isMoving)
            {
                // 점프 시작 프레임
                if (!hasDirection)
                {
                    totalMoveTime = monster.animator
                        .runtimeAnimatorController
                        .animationClips
                        .First(c => c.name == "Jump").length;

                    moveStartTime = Time.time;

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

                    // 회전 (Y축만)
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    Vector3 euler = targetRot.eulerAngles;
                    euler.x = 0f;
                    euler.z = 0f;
                    monster.transform.rotation = Quaternion.Euler(euler);

                    lastRotation = monster.transform.rotation;
                    hasDirection = true;
                }

                // 점프 이동 처리
                float t = (Time.time - moveStartTime) / totalMoveTime;
                t = Mathf.Clamp01(t);

                float speedFactor;

                if (t < 0.5f)
                {
                    float localT = t / 0.5f;
                    speedFactor = 1f - Mathf.Pow(2f, -10f * localT);
                }
                else
                {
                    float localT = (t - 0.5f) / 0.5f;
                    speedFactor = (1f - localT) * (1f - localT);
                }

                monster.moveVector = dir * monster.status.moveSpeed * speedFactor;
            }
            else
            {
                // 점프 종료
                monster.moveVector = Vector3.zero;
                hasDirection = false;
                monster.transform.rotation = lastRotation;
            }
        }
        else
        {
            monster.animator.SetBool("isMove", false);
            monster.moveVector = Vector3.zero;
            monster.transform.rotation = lastRotation;
            hasDirection = false;

            monster.StateMachine.ChangeState(monster.AttackState);
        }
    }

    public void OnExit()
    {
        monster.moveVector = Vector3.zero;
        hasDirection = false;
    }
}
