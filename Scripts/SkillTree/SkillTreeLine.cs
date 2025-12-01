using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeLine : MonoBehaviour
{
    public RectTransform lineRect;

    /// <summary>
    /// 두 노드 사이에 선을 그립니다.
    /// </summary>
    public void SetLine(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float length = direction.magnitude;

        // 선의 중심 위치로 이동
        GetComponent<RectTransform>().anchoredPosition = (start + end) * 0.5f;

        // 선 길이 설정
        lineRect.sizeDelta = new Vector2(length - 100, lineRect.sizeDelta.y);

        // 회전 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
