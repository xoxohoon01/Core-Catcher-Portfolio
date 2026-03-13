using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugEventListener : MonoBehaviour
{
    public GameObject attackObject;

    public void StartAttack()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.GetSuperArmor(1.1f);

        Vector3 projectileSize = new Vector3(8, 1, 8);

        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position;
        Vector3 forwardPos = startRotation * Vector3.forward;

        ObjectPoolManager.Instance.Spawn("RoundIndicator", startPos + (forwardPos * 7f), startRotation)
            .GetComponent<RoundIndicator>().Initialize(projectileSize, 1f, 0f, 360f, 1.15f);
        ObjectPoolManager.Instance.Spawn("RoundIndicator", startPos + (forwardPos * 14f), startRotation)
            .GetComponent<RoundIndicator>().Initialize(projectileSize, 1f, 0f, 360f, 1.3f);
        ObjectPoolManager.Instance.Spawn("RoundIndicator", startPos + (forwardPos * 21f), startRotation)
            .GetComponent<RoundIndicator>().Initialize(projectileSize, 1f, 0f, 360f, 1.45f);
    }

    public void Attack()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.StartCoroutine(controller.AttackInitialize());
    }

    public void EndAttack()
    {
        DraugController controller = GetComponent<DraugController>();
        controller.isAttack = false;
    }

    public void Death()
    {
        DraugController controller = GetComponent<DraugController>();

    }
}
