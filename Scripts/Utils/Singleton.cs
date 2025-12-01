using System;
using UnityEngine;

public class Singleton<T> where T : class
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Activator.CreateInstance(typeof(T)) as T;
            }
            return instance;
        }
    }
}
