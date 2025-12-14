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
            minutes.text = (GameManager.Instance.time / 60).ToString(@"00");
            seconds.text = (GameManager.Instance.time - ((int)(GameManager.Instance.time / 60) * 60)).ToString(@"00");
        }
    }
}
