using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.Hide<SkillTree>();
        UIManager.Instance.Hide<Settings>();
        UIManager.Instance.Hide<StagePanel>();
        UIManager.Instance.Show<CharacterSelect>("HUD");

        transform.parent.GetComponent<UpperBar>().MakeGray();
        transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    private void Start()
    {
        UIManager.Instance.Show<CharacterSelect>("HUD").Initialize();
    }
}
