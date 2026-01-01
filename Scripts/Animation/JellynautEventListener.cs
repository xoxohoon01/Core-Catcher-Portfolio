using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellynautEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JellynautController controller = GetComponent<JellynautController>();

        controller.attackIndicator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        JellynautController controller = GetComponent<JellynautController>();
        
        controller.AttackInitialize();
    }

    public void EndAttack()
    {
        JellynautController controller = GetComponent<JellynautController>();
        controller.isAttack = false;
        controller.attackIndicator.gameObject.SetActive(false);
    }
}
