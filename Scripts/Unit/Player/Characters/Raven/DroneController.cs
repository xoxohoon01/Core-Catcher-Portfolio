using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControlelr : MonoBehaviour
{
    private Transform raven;
    private Vector3 originPosition;
    Vector3 offset = new Vector3(0, 4, 0);

    private float fireDelay;

    private void Awake()
    {
        raven = transform.parent;
        originPosition = raven.position + offset;
    }

    private void Update()
    {
        Vector3 targetPos = (raven.position + offset) + ((originPosition - (raven.position + offset)).normalized * 3f);
        float dist = Vector3.Distance(originPosition, raven.position);

        if (dist > 5)
        {
            Vector3 targetVector = Vector3.Lerp(originPosition, targetPos, 2 * Time.deltaTime);
            transform.position = targetVector;
            originPosition = transform.position;
        }
        else
        {
            transform.position = originPosition;
        }

        fireDelay = Mathf.Max(fireDelay - Time.deltaTime, 0);

        if (fireDelay <= 0)
        {
            ObjectPoolManager.Instance.Spawn("NapalmBullet", transform.position, Quaternion.identity).GetComponent<NapalmBulletController>().Initialize();
            fireDelay = 3f;
        }
    }
}
