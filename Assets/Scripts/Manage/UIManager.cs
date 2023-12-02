using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [Title("Key")]
    [SerializeField] KeyCode devMenuKey = KeyCode.Escape;
    [SerializeField] KeyCode inventoryKey = KeyCode.I;
    [SerializeField] KeyCode playerStatesKey = KeyCode.C;
    [SerializeField] KeyCode questKey = KeyCode.J;

    [Title("UI")]
    public DeveloperMenu devMenu;
    public InventoryUI inventoryUI;
    public PlayerStatesUI playerStateUI;
    public QuestUI questUI;
    public StateUpgradeUI stateUpgradeUI;

    public LobbyUI curUI;

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
        if (GameManager.instance.isAllowDevMenu)
        {
            UIManage(devMenuKey, devMenu);
        }
        UIManage(inventoryKey, inventoryUI);
        UIManage(playerStatesKey, playerStateUI);
        UIManage(questKey, questUI);
    }

    private void UIManage(KeyCode key, LobbyUI ui)
    {
        if (!Input.GetKeyDown(key)) return;

        if (ui.isOpen)
        {
            ui.HideUI();

            if (curUI == ui)
            {
                curUI = null;
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

    public void UIManage(LobbyUI ui)
    {
        if (ui == null)
        {
            throw new System.NullReferenceException();
        }

        if (ui.isOpen)
        {
            ui.HideUI();

            if (curUI == ui)
            {
                curUI = null;
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
}
