using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum HUDType : byte { Exp, Timer, KillCount, Level }
    [EnumToggleButtons]
    public HUDType type;

    private TextMeshProUGUI m_Text;
    private Slider m_Slider;

    void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        m_Slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        switch (type)
        {
            case HUDType.Exp:
                float curExp = GameManager.instance.curExp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                m_Slider.value = curExp / maxExp;
                break;
            case HUDType.Timer:
                int min = Mathf.FloorToInt(GameManager.instance.timer / 60);
                int sec = Mathf.FloorToInt(GameManager.instance.timer % 60);
                m_Text.text = $"{min:D2}:{sec:D2}";
                break;
            case HUDType.KillCount:
                m_Text.text = $"Kill:{GameManager.instance.killCount}";
                break;
            case HUDType.Level:
                m_Text.text = $"Lv.{GameManager.instance.level}";
                break;
        }
    }
}
