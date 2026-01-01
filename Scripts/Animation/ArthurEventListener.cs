using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArthurEventListener : MonoBehaviour
{
    bool isAttack = false;
    bool isSkill1 = false;
    bool isSkill3 = false;
    float factor = 0;
    float span = 0;
    float targetSpeed = 0;
    float frictionTime = 0;

    private void Update()
    {
        if (GetComponent<ArthurController>().dashSpan > 0)
        {
            isAttack = false;
            isSkill1 = false;
            isSkill3 = false;
            return;
        }

        if (isAttack)
        {
            span += Time.deltaTime;
            factor = Mathf.Lerp(targetSpeed, 0, span / frictionTime);
            GetComponent<ArthurController>().moveVector = transform.forward * factor;
            if (span >= frictionTime)
            {
                isAttack = false;
                span = 0;
                factor = 0;
            }
        }

        if (isSkill1)
        {
            span += Time.deltaTime;
            factor = Mathf.Lerp(targetSpeed, 0, span / frictionTime);
            GetComponent<ArthurController>().moveVector = transform.forward * factor;
            if (span >= frictionTime)
            {
                GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                isSkill1 = false;
                span = 0;
                factor = 0;
            }
        }

        if (isSkill3)
        {
            span += Time.deltaTime;
            factor = Mathf.Lerp(targetSpeed, 0, span / frictionTime);
            GetComponent<ArthurController>().moveVector = transform.forward * factor;
            if (span >= frictionTime)
            {
                GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                isSkill3 = false;
                span = 0;
                factor = 0;
            }
        }
    }

    public void Attack1Start()
    {
        isAttack = true;
        isSkill1 = false;
        isSkill3 = false;
        factor = 0;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }

    public void Attack2Start()
    {
        isAttack = true;
        isSkill1 = false;
        isSkill3 = false;
        factor = 0;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }
    public void Attack3Start()
    {
        isAttack = true;
        isSkill1 = false;
        isSkill3 = false;
        factor = 0;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }
    public void Attack4Start()
    {
        isAttack = true;
        isSkill1 = false;
        isSkill3 = false;
        factor = 0;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1;
    }

    public void Skill1Start()
    {
        isAttack = false;
        isSkill1 = true;
        isSkill3 = false;
        factor = 0;
        span = 0;
        targetSpeed = 5;
        frictionTime = 1f;
        GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Skill3Start()
    {
        isAttack = false;
        isSkill1 = false;
        isSkill3 = true;
        factor = 0;
        span = 0;
        targetSpeed = 40;
        frictionTime = 0.5f;
        GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

}
