using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private Slider expSlider;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelText;

    void Awake()
    {
        gameManager = GameManager.instance;

        AssignUIObjects();
    }

    // 할당되지 않은 UI 할당 메소드
    private void AssignUIObjects()
    {
        // Slider 중 하나라도 참조가 안되어 있다면
        if (!expSlider || !healthSlider)
        {
            var sliders = GetComponentsInChildren<Slider>();
            if (!expSlider)
            {
                expSlider = sliders[0];
            }
            if (!healthSlider)
            {
                healthSlider = sliders[1];
            }
        }
        // Text 중 하나라도 참조가 안되어 있다면
        if (!killCountText || !timerText || !levelText)
        {
            var hudTexts = GetComponentsInChildren<TextMeshProUGUI>();
            if (!timerText)
            {
                timerText = hudTexts[0];
            }
            if (!killCountText)
            {
                killCountText = hudTexts[1];
            }
            if (!levelText)
            {
                levelText = hudTexts[2];
            }
        }
    }

    void Start()
    {
        UpdateHUD();

        gameManager.OnExpChanged += UpdateExpSlider;
        gameManager.OnKillCountChanged += UpdateKillCountText;
        gameManager.OnLevelChanged += UpdateLevelText;
        gameManager.OnHealthChanged += UpdateHealthSlider;
        StartCoroutine(UpdateTimerRoutine());
    }

    private void UpdateHUD()
    {
        UpdateExpSlider();
        UpdateKillCountText();
        UpdateLevelText();
        UpdateTimerText();
        UpdateHealthSlider();
    }

    public void UpdateExpSlider()
    {
        float curExp = gameManager.curExp;
        float maxExp = gameManager.nextExp[Mathf.Min(gameManager.level, gameManager.nextExp.Length - 1)];
        expSlider.value = curExp / maxExp;
    }

    public void UpdateTimerText()
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
            UpdateTimerText();
        }
    }

    public void UpdateKillCountText()
    {
        killCountText.text = $"Kill:{gameManager.killCount}";
    }

    public void UpdateLevelText()
    {
        levelText.text = $"Lv.{gameManager.level}";
    }

    public void UpdateHealthSlider()
    {
        float curHealth = GameManager.instance.health;
        float curMaxHealth = GameManager.instance.maxHealth;
        healthSlider.value = curHealth / curMaxHealth;
    }

    void OnDestroy()
    {
        gameManager.OnExpChanged -= UpdateExpSlider;
        gameManager.OnKillCountChanged -= UpdateKillCountText;
        gameManager.OnLevelChanged -= UpdateLevelText;
    }
}
