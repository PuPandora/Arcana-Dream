using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateUpgradeUI : LobbyUI
{
    private StateSelectButton[] stateSelectButtons;
    public StateUpgradeData selectStateData;

    [Title("State Upgrade Info UI")]
    [SerializeField] Image stateIcon;
    [SerializeField] TextMeshProUGUI stateDescText;

    [Title("State Upgrade Button")]
    [SerializeField] Button stateUpgradeButton;
    [SerializeField] TextMeshProUGUI stateUpgradeButtonText;

    public event Action OnStateUpgrade;

    protected override void Awake()
    {
        base.Awake();
        stateSelectButtons = GetComponentsInChildren<StateSelectButton>();
    }

    protected override void Initialize()
    {
        UIManager.instance.stateUpgradeUI = this;

        // 스탯 버튼 레벨 업데이트
        foreach (var button in stateSelectButtons)
        {
            button.UpdateLevel();
            OnStateUpgrade += button.UpdateLevel;
        }
    }

    protected override void Start()
    {
        foreach(var button in  stateSelectButtons)
        {
            button.Initialize();
        }
        base.Start();
    }

    public override void ShowUI()
    {
        UpdateInfo();
        base.ShowUI();
    }

    public override void HideUI()
    {
        selectStateData = null;
        base.HideUI();
    }

    public void UpdateInfo()
    {
        if (selectStateData == null)
        {
            stateIcon.enabled = false;
            stateDescText.gameObject.SetActive(false);
            return;
        }
        else
        {
            stateIcon.enabled = true;
            stateDescText.enabled = true;
            stateDescText.gameObject.SetActive(true);
        }

        stateIcon.sprite = selectStateData.icon;
        stateUpgradeButtonText.text = string.Empty;

        var level = selectStateData.level;
        if (level >= selectStateData.maxLevel)
        {
            level = (byte)(selectStateData.maxLevel - 1);
        }
        var table = selectStateData.stateUpgradeTables[level];
        var cost = selectStateData.stateUpgradeTables[level].cost;

        // 돈이 부족한 경우
        if (cost > GameManager.instance.gold)
        {
            stateUpgradeButtonText.text += $"스탯 업그레이드\n{cost}";
            stateUpgradeButton.enabled = false;
            stateUpgradeButtonText.color = Color.red;
            return;
        }
        // 최대 레벨의 스탯의 경우
        else if (selectStateData.level >= selectStateData.maxLevel - 1)
        {
            UpdateDescription(table);
            stateUpgradeButtonText.text = $"최대 레벨";
            stateDescText.text = $"최대 레벨";
            stateUpgradeButton.enabled = false;
        }
        // 업그레이드가 가능한 경우
        else
        {
            UpdateDescription(table);
            stateUpgradeButtonText.color = Color.black;
            stateUpgradeButtonText.text += $"스탯 업그레이드\n{cost}";
            stateUpgradeButton.enabled = true;
        }

        void UpdateDescription(StateUpgradeTable table)
        {
            stateDescText.text = string.Empty;

            if (table.increaseValue > 0 && table.moreValue > 0)
            {
                stateDescText.text = string.Format(
                    table.desc, 
                    selectStateData.stateName,
                    table.increaseValue, 
                    table.moreValue);
            }

            else if (table.increaseValue > 0)
            {
                stateDescText.text = string.Format(table.desc, selectStateData.stateName, table.increaseValue);
            }

            else if (table.moreValue > 0)
            {
                stateDescText.text = string.Format(
                    table.desc,
                    selectStateData.stateName,
                    table.moreValue);
            }
        }
    }

    public void UpgradeState()
    {
        var gold = GameManager.instance.gold;
        var cost = selectStateData.stateUpgradeTables[selectStateData.level].cost;

        GameManager.instance.SetGold(gold - cost);

        selectStateData.ApplyState(selectStateData.level++);

        UpdateInfo();
        OnStateUpgrade?.Invoke();
        Debug.Log($"{selectStateData.stateName} 스탯 업그레이드 완료.\n레벨 : {selectStateData.level}");
    }
}
