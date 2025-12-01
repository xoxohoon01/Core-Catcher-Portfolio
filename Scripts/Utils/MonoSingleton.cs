using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool isDontDestroyOnLoad;

    private static T instance;
    public static T Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!instance)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    GameObject newMonoSingleton = new GameObject(typeof(T).ToString());
                    newMonoSingleton.AddComponent<T>();
                    instance = newMonoSingleton.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
        if (instance == null)
        {
            instance = GetComponent<T>();
            if (isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
