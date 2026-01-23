using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPPercentage : MonoBehaviour
{
    public TMP_Text exp;
    private PlayerController character;

    private void Start()
    {
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
            UpdateGauge((int)((character.status.exp / character.status.maxExp) * 100f));
        }
    }

    public void UpdateGauge(int amount)
    {
        if (exp == null) return;

        exp.text = $"{amount.ToString()}%";
    }
}
