using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public float time = 0;
    public string characterName;

    [Range(0.0f, 30.0f)]
    public float timeScale = 1.0f;

    private void Update()
    {
        time += Time.deltaTime;

        if (Input.GetButtonDown("Menu") && SceneManager.GetActiveScene().name == "MainScene")
        {
            MenuButtons buttons = UIManager.Instance.Get<MenuButtons>();
            if (buttons == null || (buttons != null && !buttons.gameObject.activeInHierarchy))
            {
                UIManager.Instance.Show<MenuButtons>();
            }
            else
            {
                UIManager.Instance.Get<MenuButtons>().Continue();
            }
        }

        if (!BattleManager.Instance.isStop)
            Time.timeScale = timeScale;
    }

    private void Start()
    {
        characterName = "Raven";
    }
}
