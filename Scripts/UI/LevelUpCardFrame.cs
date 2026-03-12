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

        var levelUpCards = GetAvailableStatCards();
        var artifactCards = GetAvailableArtifactCards();

        Shuffle(levelUpCards);
        Shuffle(artifactCards);

        int levelIndex = 0;
        int artifactIndex = 0;

        // Slot 0: Artifact guaranteed
        if (artifactIndex < artifactCards.Count)
        {
            cardSlots[0].InitializeArtifact(artifactCards[artifactIndex]);
            artifactIndex++;
        }
        else if (levelIndex < levelUpCards.Count)
        {
            cardSlots[0].InitializeLevelUp(levelUpCards[levelIndex]);
            levelIndex++;
        }

        // Slot 1: LevelUp guaranteed
        if (levelIndex < levelUpCards.Count)
        {
            cardSlots[1].InitializeLevelUp(levelUpCards[levelIndex]);
            levelIndex++;
        }
        else if (artifactIndex < artifactCards.Count)
        {
            cardSlots[1].InitializeArtifact(artifactCards[artifactIndex]);
            artifactIndex++;
        }

        // Slot 2: Random from both pools
        bool spawnArtifact = Random.value < 0.5f;

        if (spawnArtifact && artifactIndex < artifactCards.Count)
        {
            cardSlots[2].InitializeArtifact(artifactCards[artifactIndex]);
            artifactIndex++;
        }
        else if (levelIndex < levelUpCards.Count)
        {
            cardSlots[2].InitializeLevelUp(levelUpCards[levelIndex]);
            levelIndex++;
        }
        else if (artifactIndex < artifactCards.Count)
        {
            cardSlots[2].InitializeArtifact(artifactCards[artifactIndex]);
            artifactIndex++;
        }
    }

    private List<StatCardScriptableObject> GetAvailableStatCards()
    {
        var allCards = Resources.LoadAll<StatCardScriptableObject>("Cards");

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Initialize();
        }
    }
}