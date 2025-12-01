using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellynautEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JellynautController controller = transform.parent.GetComponent<JellynautController>();

        controller.attackIndicator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        JellynautController controller = transform.parent.GetComponent<JellynautController>();
        
        controller.AttackInitialize();
    }

    public void EndAttack()
    {
        JellynautController controller = transform.root.GetComponent<JellynautController>();
        controller.isAttack = false;
        controller.attackIndicator.gameObject.SetActive(false);
    }
}
