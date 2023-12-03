using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField] private StageData data;

    public void OnClick()
    {
        UIManager.instance.stageSelectUI.selectedStage = data;
    }
}
