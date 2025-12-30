using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArtifactCardFrame : UIBase
{
    public ArtifactCard[] cardSlots = new ArtifactCard[3]; // 총 3개의 카드 슬롯 (자식 컴포넌트)
    private List<int> usedIndices = new List<int>();

    public override void Initialize()
    {
        base.Initialize();

        List<ArtifactCardScriptableObject> allCards = new List<ArtifactCardScriptableObject>();
        ArtifactCardScriptableObject[] characterArtifactCards = Resources.LoadAll<ArtifactCardScriptableObject>($"Artifacts/{GameManager.Instance.characterName}");
        foreach (ArtifactCardScriptableObject card in characterArtifactCards)
        {
            allCards.Add(card);
        }

        ArtifactCardScriptableObject[] commonArtifactCards = Resources.LoadAll<ArtifactCardScriptableObject>($"Artifacts/Common");
        foreach (ArtifactCardScriptableObject card in commonArtifactCards)
        {
            allCards.Add(card);
        }

        ShowCards();

        // 1. 사용 가능한 카드만 필터링 (레벨 < 5)
        List<ArtifactCardScriptableObject> availableCards = new List<ArtifactCardScriptableObject>();
        foreach (var card in allCards)
        {
            if (card.effectName != "HealHalf" && CardManager.Instance.artifactEffectLevel[card.effectName] < 5)
            {
                availableCards.Add(card);
            }
        }
        // 2. 카드를 섞기
        Shuffle(availableCards);

        // 3. 슬롯별로 할당
        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (i < availableCards.Count)
            {
                cardSlots[i].Initialize(availableCards[i].effectName); // 일반 카드
            }
            else
            {
                cardSlots[i].Initialize("HealHalf"); // 회복 카드
            }
        }
    }

    public void HideCards()
    {

    }

    public void ShowCards()
    {
        for (int i = 0; i < 3; i++)
        {
            RectTransform rectTransform = cardSlots[i].GetComponent<RectTransform>();

            Vector2 targetPosition = new Vector2(cardSlots[i].GetComponent<RectTransform>().anchoredPosition.x, 0);
            Vector2 startPos = new Vector2(cardSlots[i].GetComponent<RectTransform>().anchoredPosition.x, 1080);

            rectTransform.anchoredPosition = startPos;

            rectTransform.DOAnchorPos(targetPosition, 0.5f)
                .SetEase(Ease.InOutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    // 애니메이션이 완료된 후 실행할 코드 (예: Debug.Log("도착!"))
                    Debug.Log(gameObject.name + "이(가) 화면 중앙에 도착했습니다.");
                });
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
