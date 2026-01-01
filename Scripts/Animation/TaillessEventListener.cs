using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaillessEventListener : MonoBehaviour
{
    public void Move()
    {
        TaillessController controller = GetComponent<TaillessController>();
        controller.isMoving = true;
    }

    public void EndMove()
    {
        TaillessController controller = GetComponent<TaillessController>();
        controller.isMoving = false;
    }

    public void Attack()
    {
        TaillessController controller = GetComponent<TaillessController>();
        
        controller.AttackInitialize();
    }

    public void EndAttack()
    {
        TaillessController controller = GetComponent<TaillessController>();
        controller.isAttack = false;
    }
}
