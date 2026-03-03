using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LevelUpCard : MonoBehaviour, IPointerClickHandler
{
    public Image cardImage;
    public TMP_Text title;
    public TMP_Text context;

    private LevelUpCardScriptableObject levelUpCard;
    private ArtifactCardScriptableObject artifactCard;

    private bool isArtifact;

    public void InitializeLevelUp(LevelUpCardScriptableObject card)
    {
        isArtifact = false;
        levelUpCard = card;

        cardImage.sprite = card.cardSprite;
        title.text = card.displayName;

        int level = CardManager.Instance.levelUpEffectLevel[card.effectName];
        float amount = (level == 4) ? card.amountByMaxLevel : card.amount;

        context.text = string.Format(card.displayDescription, amount);
    }

    public void InitializeArtifact(ArtifactCardScriptableObject card)
    {
        isArtifact = true;
        artifactCard = card;

        cardImage.sprite = card.cardSprite;
        title.text = card.displayName;

        int level = CardManager.Instance.artifactEffectLevel[card.effectName];
        float amount = (level == 4) ? card.baseAmount + (level * card.amountPerLevel) + (card.amountByMaxLevel) : card.baseAmount + level * card.amountPerLevel;

        context.text = string.Format(card.displayDescription, amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isArtifact)
        {
            string effectName = artifactCard.effectName;

            int level = CardManager.Instance.artifactEffectLevel[effectName];
            if (level == 0)
            {
                var effect = CardManager.Instance.CreateArtifactEffect(effectName);
                effect?.ApplyEffect(artifactCard);
            }

            // 레벨 증가
            CardManager.Instance.artifactEffectLevel[effectName]++;

            UIManager.Instance.Hide<LevelUpCardFrame>();
            BattleManager.Instance.isStop = false;
            Time.timeScale = 1;
        }
        else
        {
            CardManager.Instance.levelUpEffectLevel[levelUpCard.effectName]++;
            var effect = CardManager.Instance.CreateLevelUpEffect(levelUpCard.effectName);
            effect.ApplyEffect(levelUpCard);
        }

        UIManager.Instance.Hide<LevelUpCardFrame>();
        BattleManager.Instance.isStop = false;
        Time.timeScale = 1;
    }
}