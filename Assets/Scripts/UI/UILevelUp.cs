using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUp : MonoBehaviour
{
    public Button[] levelUpButtons;
    public WeaponData[] weaponData;
    private RectTransform rect;
    [SerializeField] private RectTransform levelUpButtonGroupTransform;

    public Action OnShowUI;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        levelUpButtons = GetComponentsInChildren<Button>();
    }

    void Start()
    {
        StageManager.instance.OnLevelChanged += ShowUI;
        InitializeButtons();
    }

    private void ShowUI()
    {
        rect.localScale = Vector3.one;
        OnShowUI?.Invoke();
    }
    
    private void HideUI()
    {
        rect.localScale = Vector3.zero;
    }

    private void InitializeButtons()
    {
        var btns = GetComponentsInChildren<Button>();
        
        for (int i = 0; i < btns.Length; i++)
        {
            int tmp = i;
            btns[tmp].onClick.AddListener(GameManager.instance.Resume);
            btns[tmp].onClick.AddListener(HideUI);
        }
    }
}
