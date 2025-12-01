using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageCard : MonoBehaviour
{
    public Image stageImage;

    public void Initailze(StageData data)
    {
        stageImage.sprite = data.stageSprite;
    }
}
