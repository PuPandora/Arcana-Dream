using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    public StageData stageData;
    public Image stageThumbnail;
    [SerializeField] private TextMeshProUGUI stageInfoText;

    void Start()
    {
        stageInfoText.text = $"{stageData.stageName}\n권장 레벨 : {stageData.recommendLevel}\n목표 : {stageData.stageGoal}";
        stageThumbnail.sprite = stageData.stageThumbnail;

        var button = GetComponent<Button>();
        if (!button.interactable)
        {
            var color = new Color(button.colors.disabledColor.r, 
                button.colors.disabledColor.g, 
                button.colors.disabledColor.b, 
                1);

            stageThumbnail.color = color;
            stageInfoText.color = color;

            // Mask 컬러
            GetComponentsInChildren<Image>()[1].color = color;
        }
    }

    public void OnClick()
    {
        UIManager.instance.stageSelectUI.selectedStage = stageData;
    }
}
