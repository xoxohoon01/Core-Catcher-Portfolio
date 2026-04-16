using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SkillTree : UIBase
{
    public GameObject skillNodePrefab;
    public Transform nodeParent;
    public SkillTreeLine linePrefab;

    private SkillTreeData currentSkillTreeData;
    private string currentCharacter;
    private SkillNodeUI currentSkillNode;
    private SkillNode currentSkill;

    public Image detail;
    public Button activeButton;

    public Action skillValueChanged;

    private bool isOpened = false;

    public override void Opened(params object[] param)
    {
        if (!isOpened)
        {
            SkillManager.Instance.LoadSkill();
            LoadSkillTree(Resources.Load<SkillTreeData>($"SkillTreeSO/{GameManager.Instance.characterName}"));

            RectTransform rect = GetComponent<RectTransform>();
            // 시작 위치 설정
            rect.pivot = new Vector2(0, 1);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 2000);
            // 슬라이드 인 연출
            GetComponent<RectTransform>().DOAnchorPosY(-75f, 0.5f, true).SetEase(Ease.OutSine);


            isOpened = true;

            detail.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            detail.transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            activeButton.gameObject.SetActive(false);
        }
    }

    public override void Hide()
    {
        if (isOpened)
        {
            transform.SetAsLastSibling();

            // 시작 위치 설정
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75);
            // 슬라이드 인 연출
            GetComponent<RectTransform>().DOAnchorPosY(2000f, 0.5f, true).SetEase(Ease.InSine);

            isOpened = false;
        }
    }

    public void LoadSkillTree(SkillTreeData treeData)
    {
        currentSkillTreeData = treeData;
        currentCharacter = treeData.characterName;

        // 초기화
        skillValueChanged = null;
        if (nodeParent.transform.childCount > 0)
        {
            for (int i = 0; i < nodeParent.transform.childCount; i++)
            {
                Destroy(nodeParent.transform.GetChild(i).gameObject);
            }
        }
        Dictionary<int, GameObject> nodeMap = new();

        // 1. 노드 생성
        Vector2 offset = new Vector3(0, -100);
        foreach (var nodeData in treeData.skillNodes)
        {
            GameObject node = Instantiate(skillNodePrefab, nodeParent);
            node.GetComponent<RectTransform>().anchoredPosition = nodeData.position + offset;
            node.GetComponent<SkillNodeUI>().Setup(treeData.characterName, nodeData);
            nodeMap[nodeData.id] = node;
            skillValueChanged += node.GetComponent<SkillNodeUI>().UpdateVisuals;
        }

        // 2. 선 연결
        foreach (var nodeData in treeData.skillNodes)
        {
            if (nodeData.needNodeId != -1)
            {
                var from = nodeMap[nodeData.needNodeId].GetComponent<RectTransform>().anchoredPosition;
                var to = nodeMap[nodeData.id].GetComponent<RectTransform>().anchoredPosition;
                var line = Instantiate(linePrefab, nodeParent);
                line.SetLine(from, to);
                line.transform.SetAsFirstSibling();
            }
        }
    }

    public void SetCurrentSkill(SkillNodeUI node, SkillNode skill)
    {
        currentSkillNode = node;
        currentSkill = skill;

        string nameKey = GameManager.Instance.characterName + "_" + skill.name + "_" + "Name";
        string descKey = GameManager.Instance.characterName + "_" + skill.name + "_" + "Desc";
        detail.transform.GetChild(0).GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("SkillTreeTable", nameKey);
        detail.transform.GetChild(1).GetComponent<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString("SkillTreeTable", descKey);

        UpdateActiveButton();
    }

    public void UpdateActiveButton()
    {
        if (SkillManager.Instance.Skill[currentCharacter][currentSkill.id].isUnlocked)
        {
            foreach (var skill in currentSkillTreeData.skillNodes)
            {
                if (skill.needNodeId == currentSkill.id && SkillManager.Instance.Skill[currentCharacter][skill.id].isUnlocked)
                {
                    activeButton.gameObject.SetActive(false);
                    return;
                }
            }

            activeButton.gameObject.SetActive(true);
            activeButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Disable";
        }
        else
        {
            if (currentSkillNode.CanActivate())
            {
                activeButton.gameObject.SetActive(true);
                activeButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Active";
            }
            else
            {
                activeButton.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateSkill()
    {
        if (SkillManager.Instance.Skill[currentCharacter][currentSkill.id].isUnlocked)
        {
            SkillManager.Instance.Skill[currentCharacter][currentSkill.id].isUnlocked = false;
            currentSkillNode.UpdateVisuals();
        }
        else
        {
            SkillManager.Instance.Skill[currentCharacter][currentSkill.id].isUnlocked = true;
            currentSkillNode.UpdateVisuals();
        }

        SkillManager.Instance.SaveSkill(currentCharacter);
        skillValueChanged.Invoke();
        UpdateActiveButton();
    }
}
