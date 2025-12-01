using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArtifactCard : MonoBehaviour, IPointerClickHandler
{
    public Image cardImage;
    public TMP_Text title;
    public TMP_Text context;

    ArtifactCardScriptableObject currentCard;
    IArtifactCardEffect effect;

    public void Initialize(string effectName)
    {
        // 1. 모든 카드 로드
        ArtifactCardScriptableObject[] allCards = Resources.LoadAll<ArtifactCardScriptableObject>("Artifacts");

        // 2. 효과 이름으로 카드 검색
        currentCard = allCards.FirstOrDefault(card => card.effectName == effectName);

        // 3. 예외 처리: 효과 이름에 맞는 카드가 없을 경우 힐 카드로 대체
        if (currentCard == null)
            currentCard = allCards.FirstOrDefault(card => card.effectName == "HealHalf");

        // 4. 정상 카드일 경우
        effect = CardManager.Instance.CreateArtifactEffect(currentCard.effectName);

        // 5. 카드에 표시될 내용
        cardImage.sprite = currentCard.cardSprite ?? null;
        title.text = currentCard.displayName;
        if (CardManager.Instance.artifactEffectLevel[effectName] == 4)
            context.text = string.Format(currentCard.displayDescription, currentCard.amountByMaxLevel);
        else
            context.text = string.Format(currentCard.displayDescription, currentCard.amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (UIManager.Instance.Get<MenuButtons>().gameObject.activeInHierarchy)
        //    return;

        CardManager.Instance.artifactEffectLevel[currentCard.effectName]++;
        var effect = CardManager.Instance.CreateArtifactEffect(currentCard.effectName);
        effect.ApplyEffect(currentCard);

        UIManager.Instance.Hide<ArtifactCardFrame>();
        BattleManager.Instance.isStop = false;
        Time.timeScale = 1;
    }
}
