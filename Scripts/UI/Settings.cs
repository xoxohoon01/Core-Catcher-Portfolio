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
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(0, 2000);

            rect.DOAnchorPosY(-75f, 0.5f, true).SetEase(Ease.OutSine);

            isOpened = true;
        }

        // 슬라이더 초기화 시 이벤트 잠시 제거
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();

        // 값 세팅
        musicVolumeSlider.value = SettingManager.Instance.settingData.musicVolume;
        sfxVolumeSlider.value = SettingManager.Instance.settingData.sfxVolume;

        // 입력 필드도 세팅
        musicInputField.text = musicVolumeSlider.value.ToString("0.##");
        sfxInputField.text = sfxVolumeSlider.value.ToString("0.##");

        // 이벤트 다시 연결
        musicVolumeSlider.onValueChanged.AddListener(_ => UpdateMusicVolumeBySlider());
        sfxVolumeSlider.onValueChanged.AddListener(_ => UpdateSFXVolumeBySlider());
    }

    public override void Hide()
    {
        if (isOpened)
        {
            transform.SetAsLastSibling();
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -75f);
            GetComponent<RectTransform>().DOAnchorPosY(2000f, 0.5f, true).SetEase(Ease.InSine);
            isOpened = false;
        }
    }

    public void UpdateMusicVolumeBySlider()
    {
        musicInputField.text = musicVolumeSlider.value.ToString("0.##");
        SettingManager.Instance.SetMusicVolume(musicVolumeSlider.value);
    }

    public void UpdateSFXVolumeBySlider()
    {
        sfxInputField.text = sfxVolumeSlider.value.ToString("0.##");
        SettingManager.Instance.SetSFXVolume(sfxVolumeSlider.value);
    }

    public void UpdateMusicVolumeByInputField()
    {
        if (float.TryParse(musicInputField.text, out float value))
        {
            // 이벤트 잠시 제거
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.value = value;
            SettingManager.Instance.SetMusicVolume(value);
            musicVolumeSlider.onValueChanged.AddListener(_ => UpdateMusicVolumeBySlider());
        }
    }

    public void UpdateSfxVolumeByInputField()
    {
        if (float.TryParse(sfxInputField.text, out float value))
        {
            // 이벤트 잠시 제거
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.value = value;
            SettingManager.Instance.SetSFXVolume(value);
            sfxVolumeSlider.onValueChanged.AddListener(_ => UpdateSFXVolumeBySlider());
        }
    }
}