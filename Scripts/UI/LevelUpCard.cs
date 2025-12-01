using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpCard : MonoBehaviour, IPointerClickHandler
{
    public Image cardImage;
    public TMP_Text title;
    public TMP_Text context;

    LevelUpCardScriptableObject currentCard;
    ILevelUpCardEffect effect;

    public void Initialize(string effectName)
    {
        // 1. 모든 카드 로드
        LevelUpCardScriptableObject[] allCards = Resources.LoadAll<LevelUpCardScriptableObject>("Cards");

        // 2. 효과 이름으로 카드 검색
        currentCard = allCards.FirstOrDefault(card => card.effectName == effectName);

        // 3. 예외 처리: 효과 이름에 맞는 카드가 없을 경우 힐 카드로 대체
        if (currentCard == null)
            currentCard = allCards.FirstOrDefault(card => card.effectName == "HealHalf");

        // 4. 정상 카드일 경우
        effect = CardManager.Instance.CreateLevelUpEffect(currentCard.effectName);

        // 5. 카드에 표시될 내용
        cardImage.sprite = currentCard.cardSprite ?? null;
        title.text = currentCard.displayName;
        if (CardManager.Instance.levelUpEffectLevel[effectName] == 4)
            context.text = string.Format(currentCard.displayDescription, currentCard.amountByMaxLevel);
        else
            context.text = string.Format(currentCard.displayDescription, currentCard.amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (UIManager.Instance.Get<MenuButtons>().gameObject.activeInHierarchy)
        //    return;

        CardManager.Instance.levelUpEffectLevel[currentCard.effectName]++;
        var effect = CardManager.Instance.CreateLevelUpEffect(currentCard.effectName);
        effect.ApplyEffect(currentCard);

        UIManager.Instance.Hide<LevelUpCardFrame>();
        BattleManager.Instance.isStop = false;
        Time.timeScale = 1;

        CardManager.Instance.levelUpCount++;
        if (CardManager.Instance.levelUpCount % 3 == 0)
        {
            BattleManager.Instance.isStop = true;
            Time.timeScale = 0;
            UIManager.Instance.Show<ArtifactCardFrame>("FloatingUI").Initialize();
        }
    }
}
