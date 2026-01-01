using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        DraugController controller = GetComponent<DraugController>();
        controller.attackIndicator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.StartCoroutine(controller.AttackInitialize());
        controller.attackIndicator.gameObject.SetActive(false);
    }

    public void EndAttack()
    {
        DraugController controller = GetComponent<DraugController>();
        controller.isAttack = false;
    }

    public void Death()
    {
        DraugController controller = GetComponent<DraugController>();

    }
}
