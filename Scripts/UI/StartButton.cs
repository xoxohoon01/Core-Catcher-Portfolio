using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("´©¸§");
        StageData[] stageData = UIManager.Instance.Get<StagePanel>().stages;
        int stageNumber = UIManager.Instance.Get<StagePanel>().stagePage;
        GameManager.Instance.characterObjects.Clear();
        SceneLoadManager.Instance.StartCoroutine(SceneLoadManager.Instance.LoadSceneWithCallback(
            "MainScene",
            null,
            null,
            () => {
                BattleManager.Instance.Initialize(stageData[stageNumber]);
                GameManager.Instance.RefreshCharacter();
                }
            ));
    }
}
