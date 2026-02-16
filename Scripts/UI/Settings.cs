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

        // 로드
        musicVolumeSlider.value = SettingManager.Instance.settingData.musicVolume;
        sfxVolumeSlider.value = SettingManager.Instance.settingData.sfxVolume;
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
        musicInputField.text = musicVolumeSlider.value.ToString();
        SettingManager.Instance.SetMusicVolume(musicVolumeSlider.value);
    }

    public void UpdateSFXVolumeBySlider()
    {
        sfxInputField.text = sfxVolumeSlider.value.ToString();
        SettingManager.Instance.SetMusicVolume(sfxVolumeSlider.value);
    }

    public void UpdateMusicVolumeByInputField()
    {
        if (float.TryParse(musicInputField.text, out float value))
        {
            musicVolumeSlider.value = value;
            SettingManager.Instance.SetMusicVolume(value);
        }
    }

    public void UpdateSfxVolumeByInputField()
    {
        if (float.TryParse(sfxInputField.text, out float value))
        {
            sfxVolumeSlider.value = value;
            SettingManager.Instance.SetSFXVolume(value);
        }
    }
}

