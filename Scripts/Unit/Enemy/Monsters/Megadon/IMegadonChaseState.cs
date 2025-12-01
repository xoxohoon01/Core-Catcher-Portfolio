using System.Linq;
using UnityEngine;

public class IMegadonChaseState : IMonsterState
{
    private MegadonController monster;
    private bool hasDirection;
    private Quaternion lastRotation;
    private Vector3 dir;

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
