using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private StageManager stageManager;

    [SerializeField] private Slider expSlider;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelText;

    void Awake()
    {
        AssignUIObjects();
    }

    // 예외 처리 : 할당되지 않은 UI 할당 메소드
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
        stageManager = StageManager.instance;
        stageManager.hud = this;
        UpdateHUD();

        stageManager.OnExpChanged += UpdateExpSlider;
        stageManager.OnKillCountChanged += UpdateKillCountText;
        stageManager.OnLevelChanged += UpdateLevelText;
        stageManager.OnHealthChanged += UpdateHealthSlider;
        StartCoroutine(UpdateTimerRoutine());

        stageManager.itemCollector.OnGetItem += ShowGetItemUI;
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
        float curExp = stageManager.curExp;
        float maxExp = stageManager.nextExp[Mathf.Min(stageManager.level, stageManager.nextExp.Length - 1)];
        expSlider.value = curExp / maxExp;
    }

    public void UpdateTimerText()
    {
        int min = Mathf.FloorToInt(stageManager.timer / 60);
        int sec = Mathf.FloorToInt(stageManager.timer % 60);
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
        killCountText.text = $"Kill:{stageManager.killCount}";
    }

    public void UpdateLevelText()
    {
        levelText.text = $"Lv.{stageManager.level}";
    }

    public void UpdateHealthSlider()
    {
        float curHealth = stageManager.health;
        float curMaxHealth = GameManager.instance.playerStates.health; 
        healthSlider.value = curHealth / curMaxHealth;
    }

    public void ShowGetItemUI(ItemData data)
    {
        Debug.Log("Show Get Item UI");
        foreach (var item in StageManager.instance.getItemUis)
        {
            if (!item.gameObject.activeSelf)
            {
                item.Play(data.name, data.sprite);
                break;
            }
        }
    }

    void OnDestroy()
    {
        stageManager.OnExpChanged -= UpdateExpSlider;
        stageManager.OnKillCountChanged -= UpdateKillCountText;
        stageManager.OnLevelChanged -= UpdateLevelText;

        stageManager.itemCollector.OnGetItem -= ShowGetItemUI;
    }
}
