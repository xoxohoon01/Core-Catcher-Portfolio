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

        // 두 리스트를 무작위로 섞는다
        Shuffle(statCards);
        Shuffle(artifactCards);

        // --- Slot 0: 아티팩트 전용 슬롯 ---
        // GetAvailableArtifactCards에서 이미 필터링(4개 만렙, 소유 개수)을 처리해서 넘어옴
        if (artifactCards.Count > 0)
        {
            // 아티팩트 후보 중 첫 카드가 선택됨
            cardSlots[0].InitializeArtifact(artifactCards[0]);
        }

        // --- Slot 1: 스탯 카드 전용 슬롯 ---
        if (statCards.Count > 0)
        {
            cardSlots[1].InitializeLevelUp(statCards[0]);
        }

        // --- Slot 2: 랜덤 슬롯 (아티팩트 풀과 스탯 풀에서 무작위) ---
        // 0과 1 중 무작위로 뽑아서 이번 슬롯이 아티팩트일지 스탯일지 정한다
        List<bool> randomPool = new List<bool> { true, false }; // true: Artifact, false: Stat
        bool isArtifact = randomPool[Random.Range(0, 2)];

        if (isArtifact)
        {
            // 아티팩트 풀에 두 번째 카드(index 1)가 있으면 사용, 없으면 0번을 다시 활용
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

        // 사용 가능한 스탯 카드가 없으면 힐 카드로 대체
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
            // 1. 보유 4개를 아직 못 채웠을 때: 힐이 아닌 모든 아티팩트를 후보로
            result = allCards.Where(card =>
                card.effectName != "Heal" &&
                CardManager.Instance.artifactEffectLevel[card.effectName] < 5).ToList();
        }
        else
        {
            // 2. 4개를 이미 채웠을 때: 보유한 4개 중 만렙이 아닌 것만 후보로
            result = owned.Where(card =>
                CardManager.Instance.artifactEffectLevel[card.effectName] < 5).ToList();
        }

        // 3. 위 두 조건에서 후보가 하나도 없다면 (4개 다 만렙 등): 힐 카드만 넣어서 반환
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
            rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x, 200), 0.5f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .SetDelay(i * 0.1f);
        }
    }

    // UI 리롤 버튼에서 호출
    public void OnClickReroll()
    {
        Initialize();
    }
}