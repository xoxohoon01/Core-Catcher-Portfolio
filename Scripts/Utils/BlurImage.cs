using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MultiBlurImage : MonoBehaviour
{
    [System.Serializable]
    public class BlurTarget
    {
        public RawImage rawImage;   // 블러 적용할 RawImage
        public RenderTexture rt;    // BlurCamera RenderTexture
        [HideInInspector] public Material matInstance; // 인스턴스 Material
    }

    public Material blurMat;           // 원본 블러 머티리얼
    public List<BlurTarget> targets;   // 여러 RawImage 타겟

    private Canvas canvas;

    void Awake()
    {
        if (targets == null || targets.Count == 0)
            return;

        foreach (var target in targets)
        {
            if (target.rawImage == null || target.rt == null)
                continue;

            // Canvas 가져오기
            if (canvas == null)
                canvas = target.rawImage.canvas;

            // Material 인스턴스 생성 (RawImage마다 독립)
            target.matInstance = new Material(blurMat);
            target.rawImage.material = target.matInstance;

            // RenderTexture 연결
            target.rawImage.texture = target.rt;
        }
    }

    void LateUpdate()
    {
        if (targets == null || targets.Count == 0 || canvas == null)
            return;

        foreach (var target in targets)
        {
            if (target.rawImage == null || target.matInstance == null)
                continue;

            // RectTransform의 월드 코너 계산
            RectTransform rtUI = target.rawImage.rectTransform;
            Vector3[] corners = new Vector3[4];
            rtUI.GetWorldCorners(corners);

            Camera cam = canvas.renderMode == RenderMode.WorldSpace ? canvas.worldCamera : null;

            Vector2 min = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
            Vector2 max = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);

            Rect canvasRect = canvas.pixelRect;
            Vector4 uvRect = new Vector4(
                min.x / canvasRect.width,
                min.y / canvasRect.height,
                max.x / canvasRect.width,
                max.y / canvasRect.height
            );

            // Material 인스턴스에 UVRect 전달
            target.matInstance.SetVector("_UVRect", uvRect);
        }
    }
}
