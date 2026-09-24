using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : UIBase
{
    public ScrollRect scroll;
    public int stagePage = 0;

    public GameObject leftButton;
    public GameObject rightButton;

    public StageData[] stages;
    public TMP_Text stageName;

    private bool isOpened = false;

    public override void Opened(params object[] param)
    {
        if (!isOpened)
        {
            // 시작 위치 설정
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 2000);
            // 슬라이드 인 연출
            GetComponent<RectTransform>().DOAnchorPosY(-75f, 0.5f, true).SetEase(Ease.OutSine);

            isOpened = true;
        }
    }

    public override void Hide()
    {
        if (isOpened)
        {
            transform.SetAsLastSibling();

            // 시작 위치 설정
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75f);
            // 슬라이드 인 연출
            GetComponent<RectTransform>().DOAnchorPosY(2000f, 0.5f, true).SetEase(Ease.InSine);

            isOpened = false;
        }
    }

    public void RefreshUI()
    {
        if (scroll.content.childCount > 0)
        {
            scroll.content.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(scroll.content.rect.width / scroll.content.childCount) * stagePage, 0);
        }

        if (stagePage == 0)
            leftButton.SetActive(false);
        else
            leftButton.SetActive(true);

        if (stagePage == scroll.content.childCount - 1 || scroll.content.childCount <= 0)
            rightButton.SetActive(false);
        else
            rightButton.SetActive(true);

        if (stagePage < stages.Length)
        {
            stageName.text = stages[stagePage].stageDisplayName ?? "Prototype";
        }
    }

    public void PreviousPage()
    {
        stagePage -= 1;
        RefreshUI();
    }

    public void NextPage()
    {
        stagePage += 1;
        RefreshUI();
    }

    private void Start()
    {
        stages = Resources.LoadAll<StageData>("StageSO");
        GameObject stageCard = Resources.Load<GameObject>("UI/StageCard");
        foreach (StageData stage in stages)
        {
            GameObject newCard = Instantiate(stageCard, scroll.content);
            newCard.GetComponent<StageCard>().Initailze(stage);
        }

        RefreshUI();
    }

}
