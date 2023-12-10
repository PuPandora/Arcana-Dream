using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class KeyInputGuide : MonoBehaviour
{
    TextMeshProUGUI guideText;
    CanvasGroup canvasGroup;

    void Awake()
    {
        guideText = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void SetGuideText(string text)
    {
        guideText.text = text;
    }

    public void Show()
    {
        canvasGroup.DOFade(1f, 0.5f);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, 0.5f);
    }
}
