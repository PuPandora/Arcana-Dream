using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : LobbyUI
{
    [SerializeField] TextMeshProUGUI selectMapInfoText;
    public StageData selectedStage;
    
    public GameObject stageSelectButtonPrefab;
    public Transform mapListViewportContent;

    protected override void Initialize()
    {
        UIManager.instance.stageSelectUI = this;
        selectMapInfoText.text = selectMapInfoText.text.Replace("?", "").Trim() ;
        CreateStageSelectButtons();
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

    private void CreateStageSelectButtons()
    {
        int index = 0;

        // 맵 선택 버튼 생성, stageData 초기화
        foreach (StageData data in GameManager.instance.stageDataBase)
        {
            if (!data.isShow) continue;

            var go = Instantiate(stageSelectButtonPrefab, mapListViewportContent);
            go.name = $"Stage Select Button {index++}";

            var button = go.GetComponent<Button>();
            button.onClick.AddListener(UpdateInfoText);
            if (!data.isOpen)
            {
                button.interactable = false;
            }

            var stageSelectButton = go.GetComponent<StageSelectButton>();
            stageSelectButton.stageData = data;
        }
    }

    void OnDisable()
    {
        selectedStage = null;
        selectMapInfoText.text = "선택된 던전 :";
    }
}
