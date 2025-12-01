using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void OnClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
