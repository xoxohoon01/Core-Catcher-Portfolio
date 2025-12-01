using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmBulletController : MonoBehaviour
{
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize()
    {
        transform.rotation = Quaternion.Euler(Random.Range(-65f, -90f), Random.Range(0f, 360f), 0);

        rigidbody.velocity = transform.forward * 10;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity);

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (rigidbody.velocity.y < 0)
        {
            if (Physics.Raycast(ray, out hit, 1, LayerMask.GetMask("Ground")))
            {
                Explode(hit.point);
            }
        }
    }

    private void Explode(Vector3 position)
    {
        // 폭발 오브젝트 생성
        ObjectPoolManager.Instance.Spawn("Napalm", position, Quaternion.identity).GetComponent<HitController>().Initialize(10f, 0f, 0f, 0f, 3f, 1f, Faction.Player, Vector3.one);
        ObjectPoolManager.Instance.Despawn(gameObject);
    }
}
