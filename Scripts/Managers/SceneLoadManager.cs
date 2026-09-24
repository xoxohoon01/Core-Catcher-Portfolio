using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    [Header("로딩 UI 프리팹")]
    public GameObject loadingPrefab;

    private GameObject currentLoadingPanel;
    private CanvasGroup fadeGroup;
    private Slider progressBar;
    private TextMeshProUGUI loadingText;
    private TextMeshProUGUI tipText;

    [Header("랜덤 팁 목록")]
    public string[] tips;

    [Header("페이드 설정")]
    public float fadeDuration = 0.5f;

    // 외부에서 코루틴을 직접 부르지 않고 이 함수를 통해 편하게 부를 수 있습니다.
    public void LoadScene(string sceneName, Action onLoading = null, Action onComplete = null, Action onStartScene = null)
    {
        StartCoroutine(LoadSceneWithCallback(sceneName, onLoading, onComplete, onStartScene));
    }

    private void SetupUI()
    {
        if (currentLoadingPanel == null && loadingPrefab != null)
        {
            currentLoadingPanel = Instantiate(loadingPrefab);
            DontDestroyOnLoad(currentLoadingPanel);

            fadeGroup = currentLoadingPanel.GetComponent<CanvasGroup>();
            progressBar = currentLoadingPanel.GetComponentInChildren<Slider>();

            var texts = currentLoadingPanel.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                loadingText = texts[0];
                tipText = texts[1];
            }
        }
    }

    private IEnumerator LoadSceneWithCallback(string sceneName, Action onLoading = null, Action onComplete = null, Action onStartScene = null)
    {
        if (currentLoadingPanel == null) SetupUI();

        // 1. UI 초기화 및 팁 설정
        if (progressBar != null) progressBar.value = 0f;
        if (loadingText != null) loadingText.text = "0%";
        if (tipText != null && tips.Length > 0)
        {
            tipText.text = $"TIP: {tips[UnityEngine.Random.Range(0, tips.Length)]}";
        }

        // 2. 페이드 아웃 및 패널 활성화
        if (fadeGroup != null)
        {
            currentLoadingPanel.SetActive(true);
            yield return StartCoroutine(Fade(1f));
        }

        // 3. 비동기 로드 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        onLoading?.Invoke();

        // 4. 로딩 진행률 연출 (0.9까지)
        float timer = 0f;
        while (asyncLoad.progress < 0.9f)
        {
            timer += Time.unscaledDeltaTime;
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, timer * 2f);

            if (loadingText != null)
                loadingText.text = $"{Mathf.RoundToInt(progressBar.value * 100f)}%";

            yield return null;
        }

        // 5. 시각적 100% 마무리
        while (progressBar != null && progressBar.value < 1.0f)
        {
            progressBar.value = Mathf.MoveTowards(progressBar.value, 1.0f, Time.unscaledDeltaTime * 2f);
            if (loadingText != null)
                loadingText.text = $"{Mathf.RoundToInt(progressBar.value * 100f)}%";
            yield return null;
        }

        onComplete?.Invoke();

        // 잠시 대기 후 씬 전환 (너무 빠르면 로딩 화면을 못 보니까요)
        yield return new WaitForSecondsRealtime(0.2f);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone) yield return null;

        // 6. 씬 로드 완료 후 콜백 실행 (BattleManager 등 초기화)
        onStartScene?.Invoke();

        // 7. 페이드 인 (화면이 밝아짐)
        if (fadeGroup != null)
        {
            yield return StartCoroutine(Fade(0f));
        }

        currentLoadingPanel.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeGroup == null) yield break;
        float startAlpha = fadeGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = targetAlpha;
    }
}