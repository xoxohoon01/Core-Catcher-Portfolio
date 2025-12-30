using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectorButton : MonoBehaviour, IPointerClickHandler
{
    public enum Selector { Left, Right };
    public Selector selector;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selector == Selector.Left)
        {
            if (GameManager.Instance.characterName == "Arthur")
                GameManager.Instance.SelectCharacter("Raven");
        }
        else if (selector == Selector.Right)
        {
            if (GameManager.Instance.characterName == "Raven")
                GameManager.Instance.SelectCharacter("Arthur");
        }
    }
}
