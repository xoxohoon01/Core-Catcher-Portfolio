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

        skill1.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterData.characterName}/{character.characterData.skills[0].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        skill2.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterData.characterName}/{character.characterData.skills[1].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        skill3.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterData.characterName}/{character.characterData.skills[2].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");
        skill4.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterData.characterName}/{character.characterData.skills[3].skillID}") ?? Resources.Load<Sprite>("Sprites/DefaultSkillIcon");

        dash.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{character.characterData.characterName}/{character.characterData.dashName}") ?? Resources.Load<Sprite>($"Sprites/DefaultDashIcon");
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
        skill1.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skillDelay[0] / (character.characterData.skills[0].baseCooldown * (1 - character.status.cooldownReduction));
        skill2.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skillDelay[1] / (character.characterData.skills[1].baseCooldown * (1 - character.status.cooldownReduction));
        skill3.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skillDelay[2] / (character.characterData.skills[2].baseCooldown * (1 - character.status.cooldownReduction));
        skill4.transform.GetChild(2).GetComponent<Image>().fillAmount = character.skillDelay[3] / (character.characterData.skills[3].baseCooldown * (1 - character.status.cooldownReduction));

        dash.transform.GetChild(2).GetComponent<Image>().fillAmount = character.dashDelay / character.characterData.dashCooldown;
    }

    public void UpdateGauge()
    {
        hpGauge.fillAmount = character.status.hp / character.status.maxHP;
        expGauge.fillAmount = character.status.exp / character.status.maxExp;
    }
}
