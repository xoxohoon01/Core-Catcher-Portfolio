using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellynautEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        JellynautController controller = GetComponent<JellynautController>();

        float width = 4.5f * 2;
        controller.roundIndicator = ObjectPoolManager.Instance.Spawn("RoundIndicator", transform.position, transform.rotation).GetComponent<RoundIndicator>();
        controller.roundIndicator.Initialize(new Vector3(width, 1f, width), 1f, 0f, 360f, 2f);
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
