using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.Hide<StagePanel>();
        UIManager.Instance.Hide<SkillTree>();
        UIManager.Instance.Show<Settings>("HUD");

        transform.parent.GetComponent<UpperBar>().MakeGray();
        transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
