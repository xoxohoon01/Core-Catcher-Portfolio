using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugEventListener : MonoBehaviour
{
    public GameObject smashObject;
    public GameObject handAttackObject;

    public void PrepareHandAttack()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.GetSuperArmor(1.5f);

        Vector3 projectileSize = new Vector3(30, 1, 30);

        Quaternion startRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 startPos = transform.position;
        Vector3 forwardPos = startRotation * Vector3.forward;

        ObjectPoolManager.Instance.Spawn("RoundIndicator", startPos, startRotation)
            .GetComponent<RoundIndicator>().Initialize(projectileSize, 1f, 0.5f, 180f, 1.5f);
    }

    public void PrepareSmash()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.GetSuperArmor(1.5f);

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

    public void StartHandAttack()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.StartCoroutine(controller.HandAttackInitialize());
    }

    public void EndHandAttack()
    {
        DraugController controller = GetComponent<DraugController>();
        controller.isAttack = false;
    }

    public void StartSmash()
    {
        DraugController controller = GetComponent<DraugController>();

        controller.StartCoroutine(controller.SmashInitialize());
    }

    public void EndSmash()
    {
        DraugController controller = GetComponent<DraugController>();
        controller.isAttack = false;
    }

    public void Death()
    {
        DraugController controller = GetComponent<DraugController>();

    }
}
