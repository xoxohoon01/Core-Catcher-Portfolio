using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    private Image sprite;
    private PlayerController character;

    private void Start()
    {
        sprite = GetComponent<Image>();

        StartCoroutine(WaitForPlayerSetting());
    }

    private IEnumerator WaitForPlayerSetting()
    {
        yield return new WaitUntil(() => PlayerManager.Instance.GetPlayer() != null);

        character = PlayerManager.Instance.GetPlayer();
    }

    private void Update()
    {
        if (character != null)
        {
            UpdateGauge(character.status.hp / character.status.maxHP);
        }
    }

    public void UpdateGauge(float amount)
    {
        sprite.fillAmount = amount;
    }
}
