using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        DraugController controller = transform.parent.GetComponent<DraugController>();
        controller.attackIndicator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        DraugController controller = transform.parent.GetComponent<DraugController>();

        controller.StartCoroutine(controller.AttackInitialize());
        controller.attackIndicator.gameObject.SetActive(false);
    }

    public void EndAttack()
    {
        DraugController controller = transform.parent.GetComponent<DraugController>();
        controller.isAttack = false;
    }
}
