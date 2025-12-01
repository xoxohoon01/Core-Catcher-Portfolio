using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : UIBase
{
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public TMP_InputField musicInputField;
    public TMP_InputField sfxInputField;

    public AudioMixer audioMixer;

    private bool isOpened = false;

    public override void Opened(params object[] param)
    {
        if (!isOpened)
        {
            RectTransform rect = GetComponent<RectTransform>();
            // 시작 위치 설정
            rect.pivot = new Vector2(0, 1);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 2000);
            // 슬라이드 인 연출
            GetComponent<RectTransform>().DOAnchorPosY(-75f, 0.5f, true).SetEase(Ease.OutSine);


            isOpened = true;
        }

        // 동적 로드
        if (audioMixer == null)
        {
            audioMixer = Resources.Load<AudioMixer>("AudioMixer/Default");

            float musicVolume;
            float sfxVolume;

            if (audioMixer.GetFloat("MusicVolume", out musicVolume))
                musicVolumeSlider.value = DBToSlider(musicVolume);
            if (audioMixer.GetFloat("SFXVolume", out sfxVolume))
                sfxVolumeSlider.value = DBToSlider(sfxVolume);
        }
    }

    public override void Hide()
    {
        if (isOpened)
        {
            transform.SetAsLastSibling();

            // 시작 위치 설정
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75f);
            // 슬라이드 인 연출
            GetComponent<RectTransform>().DOAnchorPosY(2000f, 0.5f, true).SetEase(Ease.InSine);

            isOpened = false;
        }
    }

    public void UpdateMusicVolumeBySlider()
    {
        audioMixer.SetFloat("MusicVolume", SliderToDB(musicVolumeSlider.value));
        musicInputField.text = musicVolumeSlider.value.ToString();
    }

    public void UpdateSFXVolumeBySlider()
    {
        audioMixer.SetFloat("SFXVolume", SliderToDB(sfxVolumeSlider.value));
        sfxInputField.text = sfxVolumeSlider.value.ToString();
    }

    public void UpdateMusicVolumeByInputField()
    {
        if (float.TryParse(musicInputField.text, out float value))
        {
            musicVolumeSlider.value = value;
        }
    }

    public void UpdateSfxVolumeByInputField()
    {
        if (float.TryParse(sfxInputField.text, out float value))
        {
            sfxVolumeSlider.value = value;
        }
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
