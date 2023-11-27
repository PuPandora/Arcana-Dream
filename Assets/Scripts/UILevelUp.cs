using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUp : MonoBehaviour
{
    public Button[] levelUpButtons;

    void Awake()
    {
        levelUpButtons = GetComponentsInChildren<Button>();
    }

    void Start()
    {
        GameManager.instance.OnLevelChanged += DisplayUI;
        InitializeButtons();
        gameObject.SetActive(false);
    }

    private void DisplayUI()
    {
        gameObject.SetActive(true);
    }

    private void InitializeButtons()
    {
        var btns = GetComponentsInChildren<Button>();
        
        for (int i = 0; i < btns.Length; i++)
        {
            int tmp = i;
            btns[tmp].onClick.AddListener(GameManager.instance.Resume);
        }
    }
}
