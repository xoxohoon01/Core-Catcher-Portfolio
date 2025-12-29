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

    public void PlayAudio(string audioPath, string mixerGroup = "SFX", int priority = 128)
    {
        source.clip = Resources.Load<AudioClip>($"Sounds/{audioPath}");
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

    public void PlayAudioByType(string folderPath, string mixerGroup = "SFX", int priority = 128)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>($"Sounds/{folderPath}");
        source.clip = clips[Random.Range(0, clips.Length)];
        source.priority = priority;

        if (source.clip != null)
        {
            isPlaying = true;
            if (mixer.FindMatchingGroups(mixerGroup).Length != 0)
            {
                source.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerGroup)[0];
            }
            else
            {
                source.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerGroup)[0];
            }
            source.Play();
        }
        else
        {
            ObjectPoolManager.Instance.Despawn(gameObject);
        }
    }
}