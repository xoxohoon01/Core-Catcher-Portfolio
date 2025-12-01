using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class UIBase : MonoBehaviour
{
    public virtual void Opened(params object[] param)
    {

    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Initialize()
    {
        CreateBackground();
    }

    public void CreateBackground()
    {
        //GameObject background = Instantiate(Resources.Load<GameObject>("UI/UIBackground"), gameObject.transform);
        //background.transform.SetSiblingIndex(0);
    }
    protected virtual void Awake()
    {
        UIManager.Instance.canvasDictionary.TryAdd(transform.root.name, transform.root.GetComponent<Canvas>());
        UIManager.Instance.uiDictionary.TryAdd(GetType().ToString(), this);
    }
}

