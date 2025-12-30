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
            float totalSeconds = BattleManager.Instance.time;
            System.TimeSpan t = System.TimeSpan.FromSeconds(totalSeconds);

            minutes.text = t.Minutes.ToString("00");
            seconds.text = t.Seconds.ToString("00");
        }
    }
}
