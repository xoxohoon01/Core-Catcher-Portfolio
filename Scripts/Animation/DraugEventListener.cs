using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugEventListener : MonoBehaviour
{
    public void StartAttack()
    {
        DraugController controller = GetComponent<DraugController>();
        controller.attackIndicator.gameObject.SetActive(true);
        controller.indicator.gameObject.SetActive(true);
        controller.indicator.Initialize(0.5f, 0.05f, 20, 1f);
        controller.GetSuperArmor(1.1f);

        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position;
        Vector3 forwardPos = startRotation * Vector3.forward;

        ObjectPoolManager.Instance.Spawn("Indicator", startPos + (forwardPos * 7f), startRotation)
            .GetComponent<Indicator>().Initialize(0.5f, 0f, 360f, 1.15f);
        ObjectPoolManager.Instance.Spawn("Indicator", startPos + (forwardPos * 14f), startRotation)
            .GetComponent<Indicator>().Initialize(0.5f, 0f, 360f, 1.3f);
        ObjectPoolManager.Instance.Spawn("Indicator", startPos + (forwardPos * 21f), startRotation)
            .GetComponent<Indicator>().Initialize(0.5f, 0f, 360f, 1.45f);
    }

    public void Attack()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.StartCoroutine(controller.AttackInitialize());
        controller.attackIndicator.gameObject.SetActive(false);
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
