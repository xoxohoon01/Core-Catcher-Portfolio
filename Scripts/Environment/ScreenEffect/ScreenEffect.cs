using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffect : MonoBehaviour
{
    public string effectName;
    public float targetDecay;
    protected float span;
    protected float decay;

    public virtual void BeginEffect(float time)
    {

    }
}
