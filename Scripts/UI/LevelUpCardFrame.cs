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

        // �� ����Ʈ�� ���������� ����
        Shuffle(statCards);
        Shuffle(artifactCards);

        // --- Slot 0: ��Ƽ��Ʈ ���� ���� ---
        // GetAvailableArtifactCards���� �̹� ����(4�� ����, ���� �� ��)�� ó���ؼ� ������
        if (artifactCards.Count > 0)
        {
            // ��Ƽ��Ʈ Ȥ�� �� ī�尡 ���õ�
            cardSlots[0].InitializeArtifact(artifactCards[0]);
        }

        // --- Slot 1: ���� ī�� ���� ���� ---
        if (statCards.Count > 0)
        {
            cardSlots[1].InitializeLevelUp(statCards[0]);
        }

        // --- Slot 2: ���� ���� (��Ƽ��Ʈ Ǯ�� ���� Ǯ���� ������) ---
        // 0���� 1�� ���Կ��� ������� ���� ���� ī��� �� �ϳ��� ����
        List<bool> randomPool = new List<bool> { true, false }; // true: Artifact, false: Stat
        bool isArtifact = randomPool[Random.Range(0, 2)];

        if (isArtifact)
        {
            // ��Ƽ��Ʈ Ǯ�� ���� ī��(index 1)�� ������ ����, ������ 0���̶� �ٽ� Ȱ��
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

        // ��� ���� ī�尡 �����̸� �� ī�� ��ȯ
        if (available.Count == 0)
        {
            var healCard = allCards.FirstOrDefault(card => card.effectName == "Heal");
            if (healCard != null) available.Add(healCard);
        }
        return available;
    }

    private List<ArtifactCardScriptableObject> GetAvailableArtifactCards()
    {
        // ��� ��Ƽ��Ʈ �ε�
        List<ArtifactCardScriptableObject> allCards = new List<ArtifactCardScriptableObject>();
        allCards.AddRange(Resources.LoadAll<ArtifactCardScriptableObject>("ArtifactSO/" + GameManager.Instance.characterName));
        allCards.AddRange(Resources.LoadAll<ArtifactCardScriptableObject>("ArtifactSO/Common"));

        // ���� ���� ����(���� 1 �̻�) ��Ƽ��Ʈ
        var owned = allCards.Where(card =>
            CardManager.Instance.artifactEffectLevel.ContainsKey(card.effectName) &&
            CardManager.Instance.artifactEffectLevel[card.effectName] > 0).ToList();

        List<ArtifactCardScriptableObject> result = new List<ArtifactCardScriptableObject>();

        if (owned.Count < 4)
        {
            // 1. ���� 4���� �� �� ����� ��: ������ �ƴ� ��� ��Ƽ��Ʈ�� �ĺ�
            result = allCards.Where(card =>
                card.effectName != "Heal" &&
                CardManager.Instance.artifactEffectLevel[card.effectName] < 5).ToList();
        }
        else
        {
            // 2. 4���� �̹� ����� ��: ������ 4�� �� ������ �ƴ� �͸� �ĺ�
            result = owned.Where(card =>
                CardManager.Instance.artifactEffectLevel[card.effectName] < 5).ToList();
        }

        // 3. ���� �� ���ǿ��� �ĺ��� �ϳ��� ���ٸ� (4�� �� ���� ��): �� ī�常 �־ ��ȯ
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