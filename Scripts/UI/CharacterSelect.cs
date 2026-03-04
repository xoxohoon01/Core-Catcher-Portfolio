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
        StartCoroutine(WaitForCharacterAndRefresh());
    }

    private IEnumerator WaitForCharacterAndRefresh()
    {
        // GameManager.Instance.characterName이 준비될 때까지 대기
        while (string.IsNullOrEmpty(GameManager.Instance.characterName))
            yield return null;

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

        characterSkillPanel.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skills[0].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = character.skills[0].displayName;
        characterSkillPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = character.skills[0].description;

        characterSkillPanel.transform.GetChild(5).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skills[1].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(6).GetComponent<TMP_Text>().text = character.skills[1].displayName;
        characterSkillPanel.transform.GetChild(7).GetComponent<TMP_Text>().text = character.skills[1].description;

        characterSkillPanel.transform.GetChild(8).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skills[2].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(9).GetComponent<TMP_Text>().text = character.skills[2].displayName;
        characterSkillPanel.transform.GetChild(10).GetComponent<TMP_Text>().text = character.skills[2].description;

        characterSkillPanel.transform.GetChild(11).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/{character.skills[3].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        characterSkillPanel.transform.GetChild(12).GetComponent<TMP_Text>().text = character.skills[3].displayName;
        characterSkillPanel.transform.GetChild(13).GetComponent<TMP_Text>().text = character.skills[3].description;

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
