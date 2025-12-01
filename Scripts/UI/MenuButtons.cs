using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : UIBase
{
    public override void Opened(params object[] param)
    {
        base.Opened(param);
        Time.timeScale = 0;
        BattleManager.Instance.isStop = true;


        transform.SetAsLastSibling();

        DOTween.KillAll();
        DOTween.Clear();
        DOTween.Init();

        RectTransform rectTransform = transform.GetComponent<RectTransform>();

        Vector2 targetPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        Vector2 startPos = new Vector2(rectTransform.anchoredPosition.x, 1080);

        rectTransform.anchoredPosition = startPos;

        rectTransform.DOAnchorPos(targetPosition, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // 애니메이션이 완료된 후 실행할 코드 (예: Debug.Log("도착!"))
                Debug.Log(gameObject.name + "이(가) 화면 중앙에 도착했습니다.");
            });
    }
    public override void Hide()
    {
        base.Hide();

        if (BattleManager.Instance != null && BattleManager.Instance.isStop)
        {
            if (UIManager.Instance.Get<LevelUpCardFrame>() != null && UIManager.Instance.Get<LevelUpCardFrame>().gameObject.activeInHierarchy)
                return;
            if (UIManager.Instance.Get<ArtifactCardFrame>() != null && UIManager.Instance.Get<ArtifactCardFrame>().gameObject.activeInHierarchy)
                return;

            Time.timeScale = 1;
            BattleManager.Instance.isStop = false;
        }
    }

    public void Continue()
    {
        Hide();
    }

    public void GoToMainMenu()
    {
        Hide();
        SceneManager.LoadScene("LobbyScene");
    }
}
