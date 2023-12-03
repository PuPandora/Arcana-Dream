using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class OptionData
{
    [Range(0f, 1f)]
    public float SFXValue;
    [Range(0f, 1f)]
    public float BGMValue;
}

public class OptionUI : MonoBehaviour
{
    public DataManager dataManager;
    public OptionData optionData;

    public Slider soundEffectSlider;
    public Slider backgroundMusicSlider;

    void Start()
    {
        // DataManager 에게서 optionData 받아오기
        // 저장된 데이터가 없다면 DataManager가 기본 값(0.5)을 줌

        // 데이터를 받아오고, 그 수치를 적용시킴
        soundEffectSlider.value = optionData.SFXValue;
        backgroundMusicSlider.value = optionData.BGMValue;
    }

    public void ApplyOption()
    {
        optionData.SFXValue = soundEffectSlider.value;
        optionData.BGMValue = backgroundMusicSlider.value;

        // TO DO
        // 이 값이 게임 동안 유지되어야 하기에
        // OptionManager라는 매니저도 필요해 보임
        // 각 Audio Source 컴포넌트들은 OptionManager의 옵션을 따라 소리 value 조절
        // 그 외 해상도, 퀄리티도 같은 원리로 구현
    }

    public void Cancel()
    {
        soundEffectSlider.value = optionData.SFXValue;
        backgroundMusicSlider.value = optionData.BGMValue;

        // DataManager에게 optionData 보내기
    }
}
