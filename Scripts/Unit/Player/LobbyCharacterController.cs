using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterController : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.AddCharacterObject(gameObject);
    }
}
