using System.Linq;
using UnityEngine;

public class ITaillessChaseState : IMonsterState
{
    private TaillessController monster;
    private bool hasDirection;
    private Quaternion lastRotation;
    private Vector3 dir;

    private float moveStartTime;
    private float totalMoveTime;        // isMoving true 유지 시간

    public ITaillessChaseState(TaillessController monster)
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
        if (target == null || target.isDead) return;
        monster.Target = target.transform;

        
        if (Vector3.Distance(monster.transform.position, target.transform.position) > monster.attackRange)
        {
            monster.animator.SetBool("isMove", true);
            if (monster.isMoving)
            {
                // 1회만 방향 계산
                if (!hasDirection)
                {
                    totalMoveTime = monster.animator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == "Jump").length;
                    // 이동 시작 시간 기록
                    moveStartTime = Time.time;

                    // 이동 방향
                    dir = (target.transform.position - monster.transform.position).normalized;

                    // 회전
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    Vector3 euler = targetRot.eulerAngles;
                    euler.x = 0;
                    euler.z = 0;
                    monster.transform.rotation = Quaternion.Euler(euler);

                    // 방향 저장
                    lastRotation = monster.transform.rotation;
                    hasDirection = true;
                }
                else
                {
                    // 최종 이동 벡터
                    float t = (Time.time - moveStartTime) / totalMoveTime;
                    t = Mathf.Clamp01(t);

                    // 가속
                    float speedFactor;
                    if (t < 0.5f)
                    {
                        // 0~0.5 구간: 매우 빠른 가속(EaseOutExpo)
                        float localT = t / 0.5f; // 0~1

                        if (localT <= 0f)
                            speedFactor = 0f;
                        else
                            speedFactor = 1f - Mathf.Pow(2f, -10f * localT); // EaseOutExpo
                    }
                    else
                    {
                        // 0.5~1 구간: EaseIn (천천히 감속)
                        float localT = (t - 0.5f) / 0.5f; // 0~1로 정규화
                        speedFactor = (1f - localT) * (1f - localT); // Quadratic EaseIn
                    }

                    monster.moveVector = dir * monster.status.moveSpeed * speedFactor;
                }
            }
            else
            {
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

    public void OnExit() { }
}
