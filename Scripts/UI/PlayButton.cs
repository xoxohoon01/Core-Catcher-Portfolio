using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.Show<StagePanel>("HUD", true).RefreshUI();
        transform.parent.GetComponent<UpperBar>().MakeGray();
        transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
