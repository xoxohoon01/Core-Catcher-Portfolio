using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : UIBase
{
    public override void Initialize()
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();

        Vector2 targetPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        Vector2 startPos = new Vector2(rectTransform.anchoredPosition.x, 1080);

        rectTransform.anchoredPosition = startPos;

        rectTransform.DOAnchorPos(targetPosition, 1f)
            .SetEase(Ease.InOutBack)
            .SetUpdate(true)
            .SetDelay(1)
            .OnComplete(() =>
            {
                // 애니메이션이 완료된 후 실행할 코드 (예: Debug.Log("도착!"))
                Debug.Log(gameObject.name + "이(가) 화면 중앙에 도착했습니다.");
            });
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.characterObjects.Clear();
        SceneLoadManager.Instance.LoadScene(
            "LobbyScene",
            null,
            null,
            () =>
            {
                GameManager.Instance.RefreshCharacter();
            });
    }
}
