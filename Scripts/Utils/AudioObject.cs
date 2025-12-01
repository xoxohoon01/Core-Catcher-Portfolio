using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioObject : MonoBehaviour
{
    public AudioMixer mixer;
    private AudioSource source;
    private bool isPlaying = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (!source.isPlaying)
                ObjectPoolManager.Instance.Despawn(gameObject);
        }
    }

    public void PlayAudio(string sourceName, string mixerGroup = "SFX", int priority = 128)
    {
        source.clip = Resources.Load<AudioClip>($"Sounds/{sourceName}");
        source.priority = priority;

        if (source.clip != null)
        {
            isPlaying = true;
            source.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerGroup)[0];
            source.Play();
        }
        else
        {
            ObjectPoolManager.Instance.Despawn(gameObject);
        }
    }
    
}
