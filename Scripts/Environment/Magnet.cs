using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private bool isTracing = false; // 현재 추적 중인지 여부
    private float currentSpeed = 5f; // 시작 이동 속도

    private void OnEnable()
    {
        // 오브젝트 풀에서 다시 꺼내질 때 상태 초기화
        isTracing = false;
        currentSpeed = 5f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<UnitController>() != null)
        {
            UnitController target = other.GetComponent<UnitController>();
            if (target.faction == Faction.Player)
            {
                ActivateMagnet();
                ObjectPoolManager.Instance.Despawn(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }

    private void ActivateMagnet()
    {
        ExpGem[] allGems = FindObjectsByType<ExpGem>(FindObjectsSortMode.None);

        foreach (ExpGem gem in allGems)
        {
            gem.BeginMagnet();
        }
    }
}
