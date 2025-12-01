using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsIcon : MonoBehaviour
{
    public PlayerController character;

    public Image skill1;
    public Image skill2;
    public Image skill3;
    public Image skill4;

    public Image dash;

    private void Start()
    {
        StartCoroutine(WaitForPlayer());
    }

    private IEnumerator WaitForPlayer()
    {
        yield return new WaitUntil(() => PlayerManager.Instance.GetPlayer() != null);

        character = PlayerManager.Instance.GetPlayer();
    }

    private void Update()
    {
        if (character != null)
        {
            UpdateIcon();
        }
    }

    private void UpdateIcon()
    {
        skill1.fillAmount = character.skill1Delay / (character.character.skill1Cooldown * (1 - character.status.decreaseCooldown));
        skill2.fillAmount = character.skill2Delay / (character.character.skill2Cooldown * (1 - character.status.decreaseCooldown));
        skill3.fillAmount = character.skill3Delay / (character.character.skill3Cooldown * (1 - character.status.decreaseCooldown));
        skill4.fillAmount = character.skill4Delay / (character.character.skill4Cooldown * (1 - character.status.decreaseCooldown));

        dash.fillAmount = character.dashDelay / character.character.dashCooldown;
    }
}
