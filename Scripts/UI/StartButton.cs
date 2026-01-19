using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("누름");
        StageData[] stageData = UIManager.Instance.Get<StagePanel>().stages;
        int stageNumber = UIManager.Instance.Get<StagePanel>().stagePage;
        GameManager.Instance.characterObjects.Clear();

        SceneLoadManager.Instance.LoadScene(
            "MainLabScene",
            onStartScene: () => {
                // 씬이 완전히 바뀌고 화면이 밝아지기 직전에 실행됨
                BattleManager.Instance.Initialize(stageData[stageNumber]);
                GameManager.Instance.RefreshCharacter();
                }
            );
    }
}
