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

    public void StartAttack()
    {
        TaillessController controller = GetComponent<TaillessController>();

        float width = 9f;
        controller.roundIndicator = ObjectPoolManager.Instance.Spawn("RoundIndicator", transform.position, transform.rotation).GetComponent<RoundIndicator>();
        controller.roundIndicator.Initialize(new Vector3(width, 1, width), 1f, 0.3f, 80f, 1.5f);
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
