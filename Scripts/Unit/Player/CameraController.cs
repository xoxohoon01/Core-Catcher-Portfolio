using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float followSpeed = 6f;
    private Vector3 velocity;

    private void CameraMove()
    {
        Camera cam = Camera.main;
        Vector3 targetPosition = transform.position + new Vector3(0, 14, -9.5f);
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime / 0.35f);
    }
    private void LateUpdate()
    {
        CameraMove();
    }
}
