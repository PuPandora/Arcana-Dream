using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    // TO DO
    // Change to New Input System

    //[Title("Key")]
    //[SerializeField] KeyCode devMenuKey = KeyCode.Escape;
    //[SerializeField] KeyCode inventoryKey = KeyCode.I;
    //[SerializeField] KeyCode playerStatesKey = KeyCode.C;
    //[SerializeField] KeyCode questKey = KeyCode.J;

    [Title("UI")]
    public DeveloperMenu devMenu;
    public MenuUI menuUI;
    public InventoryUI inventoryUI;
    public PlayerStatesUI playerStateUI;
    public QuestUI questUI;
    public StateUpgradeUI stateUpgradeUI;
    public StageSelectUI stageSelectUI;
    public ItemTooltip itemTooltip;

    public LobbyUI curUI;

    public event Action OnCurrentUIClosed;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        //UIManage(inventoryKey, inventoryUI);
        //UIManage(playerStatesKey, playerStateUI);
        //UIManage(questKey, questUI);
    }

    public void UIManage(LobbyUI ui)
    {
        if (ui.isOpen)
        {
            ui.HideUI();

            if (curUI == ui)
            {
                curUI = null;
                GameManager.instance.playerState = PlayerState.None;
            }
        }
        else
        {
            ui.ShowUI();

            if (curUI != null)
            {
                curUI.HideUI();
            }

            curUI = ui;
        }
    } 

    public void CloseCurrentUI()
    {
        if (curUI == null) return;

        curUI.HideUI();
        curUI = null;

        OnCurrentUIClosed?.Invoke();
    }
}
