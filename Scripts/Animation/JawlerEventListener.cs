using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawlerEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JawlerController controller = GetComponent<JawlerController>();

        controller.boxIndicator = ObjectPoolManager.Instance.Spawn("BoxIndicator", transform.position, transform.rotation).GetComponent<BoxIndicator>();
        controller.boxIndicator.Initialize(new Vector3(0.1f, 1f, 4f), 2f);
    }

    public void Attack()
    {
        JawlerController controller = GetComponent<JawlerController>();
        
        controller.AttackInitialize();
    }

    public void EndAttack()
    {
        JawlerController controller = GetComponent<JawlerController>();
        controller.isAttack = false;
    }
}
