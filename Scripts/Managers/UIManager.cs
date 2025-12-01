using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public Dictionary<string, Canvas> canvasDictionary = new Dictionary<string, Canvas>();
    public Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>();

    public void CreateCanvas(string name, int sortingOrder = 0)
    {
        Canvas newCanvas = new GameObject("Canvas_" + name).AddComponent<Canvas>();
        newCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
        newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        newCanvas.sortingOrder = sortingOrder;

        var canvasScaler = newCanvas.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 1;

        newCanvas.AddComponent<GraphicRaycaster>();
        canvasDictionary.TryAdd(name, newCanvas);
    }

    public T Show<T>(string canvasName = "FloatingUI", bool isFloating = false) where T : UIBase
    {
        if (!canvasDictionary.ContainsKey(canvasName) || canvasDictionary[canvasName] == null)
        {
            CreateCanvas(canvasName);
        }

        if (uiDictionary.ContainsKey(typeof(T).ToString()))
        {
            uiDictionary[typeof(T).ToString()].gameObject.SetActive(true);
            uiDictionary[typeof(T).ToString()].Opened();

            if (isFloating)
                uiDictionary[typeof(T).ToString()].transform.SetAsLastSibling();
            return uiDictionary[typeof(T).ToString()].GetComponent<T>();
        }
        else
        {
            GameObject ui = Resources.Load<GameObject>("UI/" + typeof(T).ToString());

            if (ui == null)
            {
                Debug.LogError("리소스를 불러올 수 없습니다.");
                return null;
            }

            GameObject newUI = Instantiate(ui, canvasDictionary[canvasName].transform);
            uiDictionary[typeof(T).ToString()].Opened();
            if (isFloating)
                uiDictionary[typeof(T).ToString()].transform.SetAsLastSibling();
            return newUI.GetComponent<T>();
        }
    }

    public void Hide<T>()
    {
        if (uiDictionary.ContainsKey(typeof(T).ToString()))
        {
            uiDictionary[typeof(T).ToString()].Hide();
        }
    }

    public T Get<T>() where T : UIBase
    {
        uiDictionary.TryGetValue(typeof(T).ToString(), out var ui);
        if (ui != null)
            return ui.GetComponent<T>();
        else
            return null;
    }

}
