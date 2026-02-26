using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpCardFrame : UIBase
{
    public LevelUpCard[] cardSlots = new LevelUpCard[3];

    public override void Initialize()
    {
        base.Initialize();

        ShowCards();

        var levelUpCards = GetAvailableLevelUpCards();
        var artifactCards = GetAvailableArtifactCards();

        Shuffle(levelUpCards);
        Shuffle(artifactCards);

        int levelIndex = 0;
        int artifactIndex = 0;

        for (int i = 0; i < cardSlots.Length; i++)
        {
            bool spawnArtifact = Random.value < 0.3f;

            // 아티팩트 선택
            if (spawnArtifact && artifactIndex < artifactCards.Count)
            {
                cardSlots[i].InitializeArtifact(artifactCards[artifactIndex]);
                artifactIndex++;
            }
            // 레벨업 선택
            else if (levelIndex < levelUpCards.Count)
            {
                cardSlots[i].InitializeLevelUp(levelUpCards[levelIndex]);
                levelIndex++;
            }
            // 레벨업이 부족하면 아티팩트로 대체
            else if (artifactIndex < artifactCards.Count)
            {
                cardSlots[i].InitializeArtifact(artifactCards[artifactIndex]);
                artifactIndex++;
            }
        }
    }

    private List<LevelUpCardScriptableObject> GetAvailableLevelUpCards()
    {
        var allCards = Resources.LoadAll<LevelUpCardScriptableObject>("Cards");

        var available = allCards
            .Where(card =>
                card.effectName != "Heal" &&
                CardManager.Instance.levelUpEffectLevel[card.effectName] < 5)
            .ToList();

        // 선택 가능한 카드가 없으면 Heal 반환
        if (available.Count == 0)
        {
            var healCard = allCards.FirstOrDefault(card => card.effectName == "Heal");
            if (healCard != null)
                available.Add(healCard);
        }

        return available;
    }

    private List<ArtifactCardScriptableObject> GetAvailableArtifactCards()
    {
        List<ArtifactCardScriptableObject> allCards = new List<ArtifactCardScriptableObject>();

        var characterCards = Resources.LoadAll<ArtifactCardScriptableObject>(
            "Artifacts/" + GameManager.Instance.characterName);

        var commonCards = Resources.LoadAll<ArtifactCardScriptableObject>(
            "Artifacts/Common");

        allCards.AddRange(characterCards);
        allCards.AddRange(commonCards);

        var available = allCards
            .Where(card =>
                card.effectName != "Heal" &&
                CardManager.Instance.artifactEffectLevel[card.effectName] < 5)
            .ToList();

        // 선택 가능한 카드가 없으면 Heal 반환
        if (available.Count == 0)
        {
            var healCard = allCards.FirstOrDefault(card => card.effectName == "Heal");
            if (healCard != null)
                available.Add(healCard);
        }

        return available;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public void ShowCards()
    {
        for (int i = 0; i < 3; i++)
        {
            RectTransform rectTransform = cardSlots[i].GetComponent<RectTransform>();

            Vector2 targetPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
            Vector2 startPos = new Vector2(rectTransform.anchoredPosition.x, 1080);

            rectTransform.anchoredPosition = startPos;

            rectTransform.DOAnchorPos(targetPosition, 0.5f)
                .SetEase(Ease.InOutBack)
                .SetUpdate(true)
                .SetDelay(i * 0.05f);
        }
    }
}