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

        var statCards = GetAvailableStatCards();
        var artifactCards = GetAvailableArtifactCards();

        // 각 리스트를 개별적으로 셔플
        Shuffle(statCards);
        Shuffle(artifactCards);

        // --- Slot 0: 아티팩트 전용 슬롯 ---
        // GetAvailableArtifactCards에서 이미 조건(4개 제한, 만렙 시 힐)을 처리해서 보내줌
        if (artifactCards.Count > 0)
        {
            // 아티팩트 혹은 힐 카드가 세팅됨
            cardSlots[0].InitializeArtifact(artifactCards[0]);
        }

        // --- Slot 1: 스탯 카드 전용 슬롯 ---
        if (statCards.Count > 0)
        {
            cardSlots[1].InitializeLevelUp(statCards[0]);
        }

        // --- Slot 2: 랜덤 슬롯 (아티팩트 풀과 스탯 풀에서 무작위) ---
        // 0번과 1번 슬롯에서 사용하지 않은 남은 카드들 중 하나를 선택
        List<bool> randomPool = new List<bool> { true, false }; // true: Artifact, false: Stat
        bool isArtifact = randomPool[Random.Range(0, 2)];

        if (isArtifact)
        {
            // 아티팩트 풀의 다음 카드(index 1)가 있으면 쓰고, 없으면 0번이라도 다시 활용
            var targetCard = artifactCards.Count > 1 ? artifactCards[1] : artifactCards[0];
            cardSlots[2].InitializeArtifact(targetCard);
        }
        else
        {
            var targetCard = statCards.Count > 1 ? statCards[1] : statCards[0];
            cardSlots[2].InitializeLevelUp(targetCard);
        }
    }

    private List<StatCardScriptableObject> GetAvailableStatCards()
    {
        var allCards = Resources.LoadAll<StatCardScriptableObject>("CardSO");
        var available = allCards
            .Where(card =>
                card.effectName != "Heal" &&
                CardManager.Instance.statLevel[card.effectName] < 5)
            .ToList();

        // 모든 스탯 카드가 만렙이면 힐 카드 반환
        if (available.Count == 0)
        {
            var healCard = allCards.FirstOrDefault(card => card.effectName == "Heal");
            if (healCard != null) available.Add(healCard);
        }
        return available;
    }

    private List<ArtifactCardScriptableObject> GetAvailableArtifactCards()
    {
        // 모든 아티팩트 로드
        List<ArtifactCardScriptableObject> allCards = new List<ArtifactCardScriptableObject>();
        allCards.AddRange(Resources.LoadAll<ArtifactCardScriptableObject>("ArtifactSO/" + GameManager.Instance.characterName));
        allCards.AddRange(Resources.LoadAll<ArtifactCardScriptableObject>("ArtifactSO/Common"));

        // 현재 보유 중인(레벨 1 이상) 아티팩트
        var owned = allCards.Where(card =>
            CardManager.Instance.artifactEffectLevel.ContainsKey(card.effectName) &&
            CardManager.Instance.artifactEffectLevel[card.effectName] > 0).ToList();

        List<ArtifactCardScriptableObject> result = new List<ArtifactCardScriptableObject>();

        if (owned.Count < 4)
        {
            // 1. 아직 4개를 다 안 골랐을 때: 만렙이 아닌 모든 아티팩트가 후보
            result = allCards.Where(card =>
                card.effectName != "Heal" &&
                CardManager.Instance.artifactEffectLevel[card.effectName] < 5).ToList();
        }
        else
        {
            // 2. 4개를 이미 골랐을 때: 보유한 4개 중 만렙이 아닌 것만 후보
            result = owned.Where(card =>
                CardManager.Instance.artifactEffectLevel[card.effectName] < 5).ToList();
        }

        // 3. 만약 위 조건에서 후보가 하나도 없다면 (4개 다 만렙 등): 힐 카드만 넣어서 반환
        if (result.Count == 0)
        {
            var healCard = allCards.FirstOrDefault(card => card.effectName == "Heal");
            if (healCard != null) result.Add(healCard);
        }

        return result;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public void ShowCards()
    {
        for (int i = 0; i < 3; i++)
        {
            RectTransform rectTransform = cardSlots[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 1080);
            rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x, 0), 0.5f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .SetDelay(i * 0.1f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R)) Initialize();
    }
}