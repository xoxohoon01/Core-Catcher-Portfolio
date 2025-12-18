using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArthurEventListener : MonoBehaviour
{
    bool isAttack = false;
    float factor = 0;
    float span = 0;
    float targetSpeed = 0;
    float frictionTime = 0;

    private void Update()
    {
        if (isAttack)
        {
            span += Time.deltaTime;
            factor = Mathf.Lerp(targetSpeed, 0, span / frictionTime);
            transform.parent.GetComponent<ArthurController>().moveVector = transform.forward * factor;
            if (span >= frictionTime)
            {
                isAttack = false;
                span = 0;
                factor = 0;
            }
        }
    }

    public void Attack1Start()
    {
        isAttack = true;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }

    public void Attack2Start()
    {
        isAttack = true;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }
    public void Attack3Start()
    {
        isAttack = true;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }
    public void Attack4Start()
    {
        isAttack = true;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }

}
