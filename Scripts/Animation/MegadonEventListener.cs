using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MegadonEventListener : MonoBehaviour
{
    public void Move()
    {
        MegadonController controller = GetComponent<MegadonController>();
        controller.isMoving = true;
    }

    public void EndMove()
    {
        MegadonController controller = GetComponent<MegadonController>();
        controller.isMoving = false;
    }

    public void StartAttack()
    {
        MegadonController controller = GetComponent<MegadonController>();

        controller.boxIndicator = ObjectPoolManager.Instance.Spawn("BoxIndicator", transform.position, transform.rotation).GetComponent<BoxIndicator>();
        controller.boxIndicator.Initialize(new Vector3(0.2f, 1f, 0.9f), 2f);
    }

    public void Attack()
    {
        MegadonController controller = GetComponent<MegadonController>();
        
        controller.AttackInitialize();
    }

    public void EndAttack()
    {
        MegadonController controller = GetComponent<MegadonController>();
        controller.isAttack = false;
        controller.StateMachine.ChangeState(controller.ChaseState);
    }
}
