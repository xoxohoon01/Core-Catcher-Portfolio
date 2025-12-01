using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    public IEnumerator LoadSceneWithCallback(string sceneName, Action onLoading = null, Action onComplete = null, Action onStartScene = null)
    {
        Debug.Log("실행");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        // 로딩 중 콜백 실행
        onLoading?.Invoke();

        // 씬이 거의 다 로드될 때까지 기다림
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("로딩 중... " + (asyncLoad.progress * 100) + "%");

            yield return null;
        }

        // 로딩 완료됨
        Debug.Log("로딩 완료. 씬 활성화 준비됨.");

        // onComplete 콜백 먼저 실행
        onComplete?.Invoke();

        // 씬 전환 완료까지 대기
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 씬 전환 이후 콜백 실행
        onStartScene?.Invoke();

    }
}
