using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingData
{
    public float musicVolume = 50f;
    public float sfxVolume = 50f;
}

public class SettingManager : MonoSingleton<SettingManager>
{
    public SettingData settingData;
    private AudioMixer audioMixer;

    protected override void Awake()
    {
        base.Awake();

        audioMixer = Resources.Load<AudioMixer>("AudioMixer/Default");
        LoadSetting();
    }

    private void Start()
    {
        ApplyAllVolume();
    }

    public void SaveSetting()
    {
        DataManager.Save("SettingData", settingData);
    }

    public void LoadSetting()
    {
        if (DataManager.Exists("SettingData"))
        {
            settingData = DataManager.Load<SettingData>("SettingData");

            Debug.Log($"music {settingData.musicVolume}");
            audioMixer.GetFloat("MusicVolume", out float musicVolume);
            Debug.Log($"{musicVolume}");

            audioMixer.SetFloat("MusicVolume", SliderToDB(settingData.musicVolume));
            audioMixer.SetFloat("SFXVolume", SliderToDB(settingData.sfxVolume));
            audioMixer.GetFloat("MusicVolume", out musicVolume);
            Debug.Log($"{musicVolume}");
        }
        else
        {
            settingData = new SettingData();
            DataManager.Save("SettingData", settingData);
        }
    }

    public void ApplyAllVolume()
    {
        if (audioMixer == null) return;

        SetMusicVolume(settingData.musicVolume);
        SetSFXVolume(settingData.sfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        settingData.musicVolume = value;
        audioMixer.SetFloat("MusicVolume", SliderToDB(value));
        DataManager.Save("SettingData", settingData);
    }

    public void SetSFXVolume(float value)
    {
        settingData.sfxVolume = value;
        audioMixer.SetFloat("SFXVolume", SliderToDB(value));
        DataManager.Save("SettingData", settingData);
    }

    public float SliderToDB(float sliderValue)
    {
        if (sliderValue <= 5f)
        {
            // 0~5 -> -80 ~ -20
            return Mathf.Lerp(-80f, -20f, sliderValue / 5f);
        }
        else if (sliderValue <= 50f)
        {
            // 5~50 -> -20 ~ 0
            return Mathf.Lerp(-20f, 0f, (sliderValue - 5f) / 45f);
        }
        else
        {
            // 50~100 -> 0 ~ 20
            return Mathf.Lerp(0f, 20f, (sliderValue - 50f) / 50f);
        }
    }

    public float DBToSlider(float db)
    {
        if (db <= -20f)
        {
            // -80 ~ -20 -> 0 ~ 5
            return Mathf.InverseLerp(-80f, -20f, db) * 5f;
        }

        else if (db <= 0f)
        {
            // -20 ~ 0 -> 5 ~ 50
            return 5f + Mathf.InverseLerp(-20f, 0f, db) * 45f;
        }

        else
        {
            // 0 ~ 20 -> 50 ~ 100
            return 50f + Mathf.InverseLerp(0f, 20f, db) * 50f;
        }
    }
}
