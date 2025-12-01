using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillNodeUI : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text skillNameText;
    public Image iconImage;
    public GameObject canUnlockOverlay;
    public GameObject lockedOverlay;

    private string currentCharacter;
    private SkillNode nodeData;

    public void Setup(string characterName, SkillNode data)
    {
        currentCharacter = characterName;

        nodeData = data;
        skillNameText.text = "";
        iconImage.sprite = nodeData.sprite;

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        // 예: 해금 여부에 따라 색상 변경
        bool unlocked = CheckUnlock(nodeData.id);
        if (unlocked)
        {
            canUnlockOverlay.SetActive(false);
            lockedOverlay.SetActive(false);
        }
        else
        {
            if (CanActivate())
            {
                canUnlockOverlay.SetActive(true);
                lockedOverlay.SetActive(false);
            }
            else
            {
                canUnlockOverlay.SetActive(false);
                lockedOverlay.SetActive(true);
            }
        }
    }

    public bool CanActivate()
    {
        if (nodeData.needNodeId != -1)
        {
            if (SkillManager.Instance.Skill[currentCharacter][nodeData.needNodeId].isUnlocked)
                return true;
            else
                return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckUnlock(int nodeDataId)
    {
        bool unlocked = SkillManager.Instance.CheckSkillUnlocked("Raven", nodeDataId);
        return unlocked;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Get<SkillTree>().SetCurrentSkill(this, nodeData);
    }
}

