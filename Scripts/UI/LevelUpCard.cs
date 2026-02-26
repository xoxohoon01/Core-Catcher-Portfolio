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
        float amount = (level == 4) ? card.amountByMaxLevel : card.amount;

        context.text = string.Format(card.displayDescription, amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isArtifact)
        {
            CardManager.Instance.artifactEffectLevel[artifactCard.effectName]++;
            var effect = CardManager.Instance.CreateArtifactEffect(artifactCard.effectName);
            effect.ApplyEffect(artifactCard);
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