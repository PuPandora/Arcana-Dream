using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageSelectUI : LobbyUI
{
    [SerializeField] TextMeshProUGUI selectMapInfoText;
    public StageData selectedStage;

    protected override void Initialize()
    {
        UIManager.instance.stageSelectUI = this;
        selectMapInfoText.text = selectMapInfoText.text.Replace("?", "").Trim() ;
        HideUI();
    }

    public void UpdateInfoText()
    {
        selectMapInfoText.text = $"선택된 던전 : {selectedStage.stageName}";
    }

    public void EnterStage()
    {
        GameManager.instance.selectStageData = selectedStage;
        GameManager.instance.EnterStage();
    }

    void OnDisable()
    {
        selectedStage = null;
        selectMapInfoText.text = "선택된 던전 :";
    }
}
