using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellynautEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JellynautController controller = GetComponent<JellynautController>();

        controller.roundIndicator = ObjectPoolManager.Instance.Spawn("RoundIndicator", transform.position, transform.rotation).GetComponent<RoundIndicator>();
        controller.roundIndicator.Initialize(new Vector3(1f, 1f, 1f), 1f, 0f, 360f, 2f);
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
    }
}
