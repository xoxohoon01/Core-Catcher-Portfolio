using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpperTextRandomizer : MonoBehaviour
{

    public TMP_Text left;
    private float leftDelay;
    public float leftPeriod;
    public List<string> leftList;
    private int leftLastIndex = -1;

    public TMP_Text right1;
    private float right1Delay;
    public float right1Period;
    private int right1LastIndex = -1;
    public List<string> right1List;


    public TMP_Text right2;
    private float right2Delay;
    public float right2Period;
    private int right2LastIndex = -1;
    public List<string> right2List;

    private void Update()
    {
        leftDelay -= Time.deltaTime;
        right1Delay -= Time.deltaTime;
        right2Delay -= Time.deltaTime;

        if (leftDelay <= 0)
        {
            leftLastIndex = SetRandomText(left, leftList, leftLastIndex);
            leftDelay = leftPeriod;
        }
        if (right1Delay <= 0)
        {
            right1LastIndex = SetRandomText(right1, right1List, right1LastIndex);
            right1Delay = right1Period;
        }
        if (right2Delay <= 0)
        {
            right2LastIndex = SetRandomText(right2, right2List, right2LastIndex);
            right2Delay = right2Period;
        }
    }

    private int SetRandomText(TMP_Text textDisplay, List<string> list, int lastIndex)
    {
        if (list == null || list.Count == 0) return -1;
        if (list.Count == 1)
        {
            textDisplay.text = list[0];
            return 0;
        }

        int newIndex = lastIndex;
        // 이전 인덱스와 다른 인덱스가 나올 때까지 반복
        while (newIndex == lastIndex)
        {
            newIndex = Random.Range(0, list.Count);
        }

        textDisplay.text = list[newIndex];
        return newIndex;
    }
}
