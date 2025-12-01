using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpperBar : MonoBehaviour
{
    public PlayButton playButton;
    public CharacterSelectButton characterSelectButton;
    public SkillTreeButton skillTreeButton;
    public SettingsButton settingsButton;
    public ExitButton exitButton;

    private void Start()
    {
        GetComponent<UpperBar>().MakeGray();
        characterSelectButton.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        characterSelectButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void MakeGray()
    {
        playButton.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
        playButton.transform.GetChild(1).gameObject.SetActive(false);
        characterSelectButton.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
        characterSelectButton.transform.GetChild(1).gameObject.SetActive(false);
        skillTreeButton.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
        skillTreeButton.transform.GetChild(1).gameObject.SetActive(false);
        settingsButton.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
        settingsButton.transform.GetChild(1).gameObject.SetActive(false);
        exitButton.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
        exitButton.transform.GetChild(1).gameObject.SetActive(false);
    }
}
