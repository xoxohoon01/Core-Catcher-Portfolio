using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text minutes;
    public TMP_Text seconds;

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            minutes.text = (BattleManager.Instance.entireTime / 60).ToString(@"00");
            seconds.text = (BattleManager.Instance.entireTime - ((int)(BattleManager.Instance.entireTime / 60) * 60)).ToString(@"00");
        }
    }
}
