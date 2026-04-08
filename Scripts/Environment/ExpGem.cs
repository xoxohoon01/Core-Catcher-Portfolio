using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public float exp;
    public float startSpeed = 5f;     // 시작 속도
    public float acceleration = 10f;  // 초당 증가할 속도 (가속도)
    public float maxSpeed = 100f;      // 제한할 최대 속도

    private bool isTracing = false; // 현재 추적 중인지 여부
    private float currentSpeed = 5f; // 시작 이동 속도

    private void OnEnable()
    {
        // 오브젝트 풀에서 다시 꺼내질 때 상태 초기화
        isTracing = false;
        currentSpeed = 5f;
    }

    private void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);

        if (PlayerManager.Instance.GetPlayer() != null)
        {
            PlayerController player = PlayerManager.Instance.GetPlayer();
            Transform playerTransform = PlayerManager.Instance.GetPlayer().transform;

            // 추적 시작 판정 (한 번 시작하면 멀어져도 유지)
            if (!isTracing)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                if (distance < player.status.magneticRange)
                {
                    isTracing = true;
                }
            }

            // 추적 로직: 시간에 따라 가속
            if (isTracing)
            {
                // 시간에 따라 속도를 계속 더함 (v = v0 + at)
                currentSpeed += acceleration * Time.deltaTime;

                // 최대 속도 제한
                if (currentSpeed > maxSpeed)
                    currentSpeed = maxSpeed;

                // 플레이어 방향으로 이동
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, currentSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<UnitController>() != null)
        {
            UnitController target = other.GetComponent<UnitController>();
            if (target.faction == Faction.Player)
            {
                PlayerManager.Instance.GetExp(exp);
                isTracing = false;
                ObjectPoolManager.Instance.Despawn(gameObject);
            }
        }
    }

    public void BeginMagnet()
    {
        isTracing = true;
    }
}
