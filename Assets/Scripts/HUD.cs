using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private GameManager gameManager;

    private Slider expSlider;
    private TextMeshProUGUI timerText;
    private TextMeshProUGUI killCountText;
    private TextMeshProUGUI levelText;

    void Awake()
    {
        gameManager = GameManager.instance;
        expSlider = GetComponentInChildren<Slider>();
        var hudTexts = GetComponentsInChildren<TextMeshProUGUI>();

        timerText = hudTexts[0];
        killCountText = hudTexts[1];
        levelText = hudTexts[2];
    }

    void Start()
    {
        UpdateHUD();

        gameManager.OnExpChanged += UpdateExp;
        gameManager.OnKillCountChanged += UpdateKillCount;
        gameManager.OnLevelChanged += UpdateLevel;
        StartCoroutine(UpdateTimerRoutine());
    }

    private void UpdateHUD()
    {
        UpdateExp();
        UpdateKillCount();
        UpdateLevel();
        UpdateTimer();
    }

    public void UpdateExp()
    {
        float curExp = gameManager.curExp;
        float maxExp = gameManager.nextExp[Mathf.Min(gameManager.level, gameManager.nextExp.Length - 1)];
        expSlider.value = curExp / maxExp;
    }

    public void UpdateTimer()
    {
        int min = Mathf.FloorToInt(gameManager.timer / 60);
        int sec = Mathf.FloorToInt(gameManager.timer % 60);
        timerText.text = $"{min:D2}:{sec:D2}";
    }

    private IEnumerator UpdateTimerRoutine()
    {
        while (true)
        {
            yield return Utils.delay0_1;
            UpdateTimer();
        }
    }

    public void UpdateKillCount()
    {
        killCountText.text = $"Kill:{gameManager.killCount}";
    }

    public void UpdateLevel()
    {
        levelText.text = $"Lv.{gameManager.level}";
    }

    void OnDestroy()
    {
        gameManager.OnExpChanged -= UpdateExp;
        gameManager.OnKillCountChanged -= UpdateKillCount;
        gameManager.OnLevelChanged -= UpdateLevel;
    }
}
