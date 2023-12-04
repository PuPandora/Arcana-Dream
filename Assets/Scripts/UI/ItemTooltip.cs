using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : LobbyUI
{
    public ItemData itemData;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    //[SerializeField] Canvas parentCanvas;

    protected override void Initialize()
    {
        UIManager.instance.itemTooltip = this;
        HideUI();
    }

    public void Update()
    {
        float x = Input.mousePosition.x - Screen.width * 0.5f;
        float y = Input.mousePosition.y - Screen.height * 0.5f;
        var tooltipSizeX = rect.sizeDelta.x * 0.5f; // 220
        var screenWidth = Screen.width * 0.5f; // 960

        // 오른쪽으로 짤리는 문제 방지
        if (x + tooltipSizeX > screenWidth - tooltipSizeX)
        {
            x = screenWidth - tooltipSizeX;
        }
        rect.transform.localPosition = new Vector3(x, y, 0);
    }

    public void UpdateText()
    {
        nameText.text = itemData.name;
        descriptionText.text = itemData.description;
    }

    public override void ShowUI()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowUIRoutine());
    }

    private IEnumerator ShowUIRoutine()
    {
        // 텍스트 크기 변경 후 대기없이 출력했을 때
        // Content Size Fitter를 써도 깨지는 현상 방지
        UpdateText();
        yield return null;

        rect.localScale = Vector3.one;
        isOpen = true;
    }
}
