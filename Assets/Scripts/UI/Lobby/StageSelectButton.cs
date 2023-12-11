using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField] private StageData data;
    [SerializeField] private TextMeshProUGUI stageInfoText;

    void Awake()
    {
        stageInfoText.text = $"{data.stageName}\n권장 레벨 : {data.recommendLevel}\n목표 : {data.stageGoal}";
    }

    public void OnClick()
    {
        UIManager.instance.stageSelectUI.selectedStage = data;
    }
}
