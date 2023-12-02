using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateSelectButton : MonoBehaviour
{
    public StateUpgradeData data;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image icon;

    public void Initialize()
    {
        if (data == null)
        {
            Debug.LogWarning("스탯 데이터가 없습니다.", gameObject);
            return;
        }
        nameText.text = data.stateName;
        levelText.text = $"LV.{data.level}";
        icon.sprite = data.icon;
    }

    public void UpdateLevel()
    {
        if (data != null)
        {
            levelText.text = $"LV.{data.level}";
        }
    }

    public void OnClick()
    {
        UIManager.instance.stateUpgradeUI.selectStateData = data;
        UIManager.instance.stateUpgradeUI.UpdateInfo();
    }
}
