using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectCard : MonoBehaviour, IPointerClickHandler
{
    CharacterScriptableObject character;

    public void Instantiate(CharacterScriptableObject targetCharacter)
    {
        character = targetCharacter;
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterName}/Card_{character.characterName}");
    }

    public void OnClick()
    {
        UIManager.Instance.Show<CharacterSelect>();
        UIManager.Instance.Get<CharacterSelect>().RefreshText(character);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Show<CharacterSelect>();
        UIManager.Instance.Get<CharacterSelect>().RefreshText(character);
        GameManager.Instance.characterName = character.characterName;
    }
}
