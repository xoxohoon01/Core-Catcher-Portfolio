using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawlerEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JawlerController controller = GetComponent<JawlerController>();
        controller.attackIndicator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        JawlerController controller = GetComponent<JawlerController>();
        
        controller.AttackInitialize();
        controller.attackIndicator.gameObject.SetActive(false);
    }

    public void EndAttack()
    {
        JawlerController controller = GetComponent<JawlerController>();
        controller.isAttack = false;
    }
}
