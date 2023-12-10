using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class OptionData
{
    // Volume
    [Range(0f, 1f)]
    public float sfxVolume = 0.5f;
    [Range(0f, 1f)]
    public float bgmVoulme = 0.5f;

    // Screen
    public bool isFullScreen = true;
    public Resolution resolution = new Resolution();
    public RefreshRate hz = new RefreshRate();
}

public class OptionUI : MonoBehaviour
{
    public DataManager dataManager;
    public OptionData optionData;

    public Slider soundEffectSlider;
    public Slider backgroundMusicSlider;
    public Button applyButton;

    // Event
    public event Action<float> OnSfxVolumeChanged;
    public event Action<float> OnBgmVolumeChanged;

    void Start()
    {
        // DataManager에서 옵션 정보 불러오기 + 적용
        DataManager.instance.LoadOptionData();
        optionData = DataManager.instance.optionData;

        soundEffectSlider.value = optionData.sfxVolume;
        backgroundMusicSlider.value = optionData.bgmVoulme;

        // 볼륨 슬라이더 변동이 일어나면 AudioManager 볼륨 조절
        OnSfxVolumeChanged += AudioManager.instance.ChangeSfxVolume;
        OnBgmVolumeChanged += AudioManager.instance.ChangeBgmVolume;

        // 적용 버튼
        applyButton.onClick.AddListener(ApplyOption);
    }

    public void ApplyOption()
    {
        optionData.sfxVolume = soundEffectSlider.value;
        optionData.bgmVoulme = backgroundMusicSlider.value;
        DataManager.instance.SaveOptionData();
    }

    public void Cancel()
    {
        soundEffectSlider.value = optionData.sfxVolume;
        backgroundMusicSlider.value = optionData.bgmVoulme;

        // DataManager에게 optionData 보내기
    }

    public void ApplySfxVolume(float value)
    {
        optionData.sfxVolume = value;
        OnSfxVolumeChanged?.Invoke(value);
    }

    public void ApplyBgmVolume(float value)
    {
        optionData.bgmVoulme = value;
        OnBgmVolumeChanged?.Invoke(value);
    }
}
