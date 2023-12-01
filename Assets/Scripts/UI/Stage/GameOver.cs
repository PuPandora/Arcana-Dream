using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI gameClearText;
    [SerializeField] DOTweenAnimation dimTween;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        StageManager.instance.OnGameOver += ShowGameOver;
        StageManager.instance.OnGameClear += ShowGameClear;
    }

    public void ShowGameOver()
    {
        ShowUI();
        gameOverText.gameObject.SetActive(true);
    }

    public void ShowGameClear()
    {
        ShowUI();
        gameClearText.gameObject.SetActive(true);
    }

    private void ShowUI()
    {
        rect.localScale = Vector3.one;
        dimTween.DOPlay();
    }
}
