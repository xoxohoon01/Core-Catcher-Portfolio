using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoSingleton<MusicManager>
{
    public AudioClip[] lobbyClips;
    public AudioClip[] mainClips;
    public bool isStop;

    private AudioSource source;

    protected override void Awake()
    {
        base.Awake();

        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (BattleManager.Instance != null && source.isPlaying)
        {
            if (BattleManager.Instance.isStop)
            {
                source.volume = Mathf.Clamp(source.volume - 0.3f * Time.fixedDeltaTime, 0.15f, 0.4f);
            }
            else
            {
                source.volume = Mathf.Clamp(source.volume + 0.3f * Time.fixedDeltaTime, 0.15f, 0.4f);
            }
        }
        if (!source.isPlaying)
        {
            Play(SceneManager.GetActiveScene().name);
        }
    }

    public void Play(string sceneName)
    {
        if (sceneName == "LobbyScene")
        {
            source.clip = lobbyClips[Random.Range(0, lobbyClips.Length)];
        }
        else if (sceneName == "MainScene")
        {
            List<WeightedItem<AudioClip>> clipList = new List<WeightedItem<AudioClip>>();
            foreach(var clip in mainClips)
            {
                if (clip == source.clip) 
                    continue;

                WeightedItem<AudioClip> weightedClip = new WeightedItem<AudioClip>()
                { 
                    item = clip,
                    weight = 1,
                    isEssential = false
                };
                clipList.Add(weightedClip);
            }

            source.clip = WeightedRandom.Choose(clipList);
        }

        source.Play();
    }

}
