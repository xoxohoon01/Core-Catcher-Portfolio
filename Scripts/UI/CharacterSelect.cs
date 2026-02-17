using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelect : UIBase
{
    private string currentCharacterName;
    public RawImage characterDetailPanel;
    public RawImage characterSkillPanel;
    public GameObject participateButton;

    public override void Initialize()
    {
        CharacterScriptableObject[] characters = Resources.LoadAll<CharacterScriptableObject>("CharacterSO");

        currentCharacterName = GameManager.Instance.characterName;
        RefreshText(currentCharacterName);
    }

    public void RefreshText(string characterName)
    {
        CharacterScriptableObject character = Resources.Load<CharacterScriptableObject>($"CharacterSO/{characterName}");

        characterDetailPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = character.characterName;
        characterDetailPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = character.characterDescription;
        characterDetailPanel.transform.GetChild(4).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.dashName}") ?? Resources.Load<Sprite>($"Sprites/DefaultDashIcon");
        characterDetailPanel.transform.GetChild(5).GetComponent<TMP_Text>().text = character.dashDisplayName;
        characterDetailPanel.transform.GetChild(6).GetComponent<TMP_Text>().text = character.dashDescription;

        characterSkillPanel.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skill1Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = character.skill1DisplayName;
        characterSkillPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = character.skill1Description;

        characterSkillPanel.transform.GetChild(5).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skill2Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(6).GetComponent<TMP_Text>().text = character.skill2DisplayName;
        characterSkillPanel.transform.GetChild(7).GetComponent<TMP_Text>().text = character.skill2Description;

        characterSkillPanel.transform.GetChild(8).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skill3Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(9).GetComponent<TMP_Text>().text = character.skill3DisplayName;
        characterSkillPanel.transform.GetChild(10).GetComponent<TMP_Text>().text = character.skill3Description;

        characterSkillPanel.transform.GetChild(11).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skill4Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(12).GetComponent<TMP_Text>().text = character.skill4DisplayName;
        characterSkillPanel.transform.GetChild(13).GetComponent<TMP_Text>().text = character.skill4Description;

        if (currentCharacterName == GameManager.Instance.characterName)
            participateButton.SetActive(false);
        else
            participateButton.SetActive(true);
    }

    public void NextCharacter()
    {
        if (currentCharacterName == "Raven")
        {
            currentCharacterName = "Arthur";
            GameManager.Instance.SelectCharacter("Arthur");
        }
    }
    public void PreviousCharacter()
    {
        if (currentCharacterName == "Arthur")
        {
            currentCharacterName = "Raven";
            GameManager.Instance.SelectCharacter("Raven");
        }
    }

    public void Select()
    {
        GameManager.Instance.characterName = currentCharacterName;
        CharacterManager.Instance.characterData.lastSelectedCharacterId = currentCharacterName;
        CharacterManager.Instance.Save();
        if (currentCharacterName == GameManager.Instance.characterName)
            participateButton.SetActive(false);
        else
            participateButton.SetActive(true);
    }
}
