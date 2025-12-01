using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MainSceneUIManager : MonoSingleton<MainSceneUIManager>
{
    private PlayerController character;

    public Image hpGauge;
    public Image expGauge;

    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;
    public GameObject skill4;

    public GameObject dash;

    private void Start()
    {
        StartCoroutine(WaitForPlayer());
    }

    private IEnumerator WaitForPlayer()
    {
        yield return new WaitUntil(() => PlayerManager.Instance.GetPlayer() != null);

        character = PlayerManager.Instance.GetPlayer();

        skill1.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.character.characterName}/{character.character.skill1Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        skill2.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.character.characterName}/{character.character.skill2Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        skill3.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.character.characterName}/{character.character.skill3Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        skill4.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.character.characterName}/{character.character.skill4Name}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");

        dash.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.character.characterName}/{character.character.dashName}") ?? Resources.Load<Sprite>($"Sprites/DefaultDashIcon");
    }

    private void Update()
    {
        if (character != null)
        {
            UpdateIcon();
            UpdateGauge();
        }
    }

    private void UpdateIcon()
    {
        skill1.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skill1Delay / (character.character.skill1Cooldown * (1 - character.status.decreaseCooldown));
        skill2.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skill2Delay / (character.character.skill2Cooldown * (1 - character.status.decreaseCooldown));
        skill3.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skill3Delay / (character.character.skill3Cooldown * (1 - character.status.decreaseCooldown));
        skill4.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skill4Delay / (character.character.skill4Cooldown * (1 - character.status.decreaseCooldown));

        dash.transform.GetChild(2).GetComponent<Image>().fillAmount = character.dashDelay / character.character.dashCooldown;
    }

    public void UpdateGauge()
    {
        hpGauge.fillAmount = character.status.hp / character.status.maxHP;
        expGauge.fillAmount = character.status.exp / character.status.maxExp;
    }
}
