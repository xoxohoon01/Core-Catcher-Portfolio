using System.Collections;
using System.Collections.Generic;
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
        controller.attackIndicator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        MegadonController controller = GetComponent<MegadonController>();
        
        controller.AttackInitialize();
        controller.attackIndicator.gameObject.SetActive(false);
    }

    public void EndAttack()
    {
        MegadonController controller = GetComponent<MegadonController>();
        controller.isAttack = false;
        controller.StateMachine.ChangeState(controller.ChaseState);
    }
}
