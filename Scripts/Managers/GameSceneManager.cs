using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        AudioSource  source = GameManager.Instance.GetComponent<AudioSource>();
        MusicManager.Instance.Play(scene.name);

        if (scene.name == "MainScene")
        {
            UIManager.Instance.CreateCanvas("Indicator", 0);
            UIManager.Instance.canvasDictionary["Indicator"].renderMode = RenderMode.WorldSpace;
        }
    }
}
