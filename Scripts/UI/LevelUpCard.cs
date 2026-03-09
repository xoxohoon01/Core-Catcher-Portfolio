using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        string desc = card.displayDescription;

        string key = "{" + card.type + "}";

        float value = (level == 4) ? card.amountByMaxLevel : card.amount;
        value = card.isModified ? value * 100 : value;

        desc = desc.Replace(key, FormatValue(value, card.isPercentage));
        context.text = GetFormattedValue(desc);
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

            string key = "{" + attr.type + "}";
            float value = card.GetNextValue(attr.type, level);

            desc = desc.Replace(key, FormatValue(value, attr.isPercentage));
        }

        context.text = GetFormattedValue(desc);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isArtifact)
            CardManager.Instance.SelectArtifact(artifactCard.effectName);
        else
            CardManager.Instance.SelectLevelUp(levelUpCard.effectName);

        UIManager.Instance.Hide<LevelUpCardFrame>();
        BattleManager.Instance.isStop = false;
        Time.timeScale = 1;
    }

    private string FormatValue(float value, bool isPercent)
    {
        if (isPercent)
            return $"<color=#80D4FF>{value:0.##%}</color>";
        else
            return $"<color=#80FF80>{value:0.##}</color>";
    }

    private string GetFormattedValue(string desc)
    {
        desc = desc.Replace("Skill Cooldown", "<color=#FA37CD>Skill Cooldown</color>");
        desc = desc.Replace("Skill Damage", "<color=#C329F2>Skill Damage</color>");
        desc = desc.Replace("Skill Range", "<color=#00FFAA>Skill Range</color>");

        desc = desc.Replace("Motion Speed", "<color=#6400FF>Motion Speed</color>");
        desc = desc.Replace("Attack Speed", "<color=#FFFF00>Attack Speed</color>");
        desc = desc.Replace("Move Speed", "<color=#00AAFF>Move Speed</color>");

        desc = desc.Replace("Crit Chance", "<color=#FF8800>Crit Chance</color>");
        desc = desc.Replace("Crit Damage", "<color=#FF6600>Crit Damage</color>");

        desc = desc.Replace("MaxHP", "<color=#00FF00>MaxHP</color>");
        desc = desc.Replace("HP", "<color=#00FF00>HP</color>");
        
        desc = desc.Replace("Shield", "<color=#0064FF>Shield</color>");

        desc = desc.Replace("Damage", "<color=#FF0000>Damage</color>");

        return desc;
    }
}