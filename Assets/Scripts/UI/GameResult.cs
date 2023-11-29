using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    RectTransform rect;
    StageManager stageManager;
    [SerializeField] private Button stageExitButton;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private DOTweenAnimation dimTween;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject weaponResultTextPrefab;
    [SerializeField] private Transform[] weaponResultTextLayouts; 

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        stageManager = StageManager.instance;
        InititalizePanelTween();
    }

    private void InititalizePanelTween()
    {
        var panelCloseTween = panel.GetComponents<DOTweenAnimation>()[1];

        panelCloseTween.onComplete.AddListener(GameManager.instance.ExitStage);
        panelCloseTween.onComplete.AddListener(GameManager.instance.Resume);
    }

    [ContextMenu("Show UI")]
    public void ShowUI()
    {
        rect.localScale = Vector3.one;
        SetResulText();
        dimTween.DOPlayById("Open");
    }

    private void SetResulText()
    {
        int min = Mathf.FloorToInt(stageManager.timer / 60);
        int sec = Mathf.FloorToInt(stageManager.timer % 60);
        string survivedTime = $"{min:D2}:{sec:D2}";

        //resultText.text = string.Format(resultText.text, survivedTime, 0, stageManager.level, stageManager.killCount);

        resultText.text = $"생존 시간 : {survivedTime}\n" +
            $"획득 골드 : {0}\n" +
            $"달성 레벨 : {stageManager.level}\n" +
            $"처치 수 : {stageManager.killCount}";

        SetWeaponResult();
    }

    private void  SetWeaponResult()
    {
        foreach (var playerWeapon in StageManager.instance.player.weapons)
        {
            if (playerWeapon.property.type == WeaponProperty.WeaponType.Gear) continue;
            
            MakeWeaponResultText(playerWeapon);
        }
    }

    private void MakeWeaponResultText(PlayerWeapon playerWeapon)
    {
        TextMeshProUGUI weaponNameText;
        TextMeshProUGUI weaponLevelText;
        TextMeshProUGUI weaponDamageText;
        var weaponName = Instantiate(weaponResultTextPrefab, weaponResultTextLayouts[0]);
        var weaponLevel = Instantiate(weaponResultTextPrefab, weaponResultTextLayouts[1]);
        var weaponDamage = Instantiate(weaponResultTextPrefab, weaponResultTextLayouts[2]);

        weaponNameText = weaponName.GetComponent<TextMeshProUGUI>();
        weaponLevelText = weaponLevel.GetComponent<TextMeshProUGUI>();
        weaponDamageText = weaponDamage.GetComponent<TextMeshProUGUI>();

        weaponNameText.text = playerWeapon.data.weaponName;
        weaponLevelText.text = ((long)playerWeapon.property.level).ToString();
        weaponDamageText.text = ((long)playerWeapon.property.totalDamage).ToString();
    }
}
