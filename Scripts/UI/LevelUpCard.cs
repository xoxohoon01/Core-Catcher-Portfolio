using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
        string desc = card.displayDescription;

        foreach (var attr in card.attributes)
        {
            if (attr.type == AttributeType.none) continue;

            float value = card.GetNextValue(attr.type, level);
            string key = "{" + attr.type + "}";

            string formattedValue = attr.isPercentage
                ? value.ToString("0.##%")
                : value.ToString("0.##");

            desc = desc.Replace(key, formattedValue);
        }

        context.text = desc;
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
                PlayerManager.Instance.GetPlayer().AddArtifact(effect, artifactCard);
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