using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawlerEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JawlerController controller = transform.parent.GetComponent<JawlerController>();
        controller.attackIndicator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        JawlerController controller = transform.parent.GetComponent<JawlerController>();
        
        controller.AttackInitialize();
        controller.attackIndicator.gameObject.SetActive(false);
    }

    public void EndAttack()
    {
        JawlerController controller = transform.root.GetComponent<JawlerController>();
        controller.isAttack = false;
    }
}
