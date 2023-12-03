using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperMenu : LobbyUI
{
    Button[] debugButtons;

    protected override void Awake()
    {
        base.Awake();

        rect = GetComponentsInParent<RectTransform>()[1];
        debugButtons = GetComponentsInChildren<Button>();
    }

    protected override void Start()
    {
        UIManager.instance.devMenu = this;
        InitializeButtons();
        base.Start();
    }

    protected override void Initialize()
    {
        UIManager.instance.devMenu = this;
        HideUI();
    }

    private void InitializeButtons()
    {
        foreach (var button in debugButtons)
        {
            button.interactable = false;
        }

        var gameManager = GameManager.instance;
        var inventoryClearBtn = debugButtons[0];
        var saveGameBtn = debugButtons[1];
        var loadGameBtn = debugButtons[2];
        var lobbyBtn = debugButtons[3];

        InitializeButton(inventoryClearBtn, gameManager.ClearInventory, "인벤토리 초기화");
        InitializeButton(saveGameBtn, gameManager.SaveGame, "게임 저장");
        InitializeButton(loadGameBtn, gameManager.LoadGame, "게임 불러오기");
        InitializeButton(lobbyBtn, gameManager.EnterLobby, "로비로 이동");

        var closeButton = debugButtons[debugButtons.Length - 1];
        InitializeButton(closeButton, () => rect.gameObject.SetActive(false), "X");
    }

    private void InitializeButton(Button button, UnityEngine.Events.UnityAction call, string text)
    {
        button.onClick.AddListener(call);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        button.interactable = true;
    }

    public override void ShowUI()
    {
        rect.localScale = Vector3.one;
        rect.gameObject.SetActive(true);
        isOpen = true;
    }

    public override void HideUI()
    {
        rect.localScale = Vector3.zero;
        rect.gameObject.SetActive(false);
        isOpen = false;
    }
}
