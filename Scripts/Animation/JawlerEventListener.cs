using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawlerEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JawlerController controller = GetComponent<JawlerController>();

        float width = 0.2f;
        float speed = 30f;
        float duration = 1f;
        controller.boxIndicator = ObjectPoolManager.Instance.Spawn("BoxIndicator", transform.position, transform.rotation).GetComponent<BoxIndicator>();
        controller.boxIndicator.Initialize(new Vector3(width, 1f, speed * duration), 2f);
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
