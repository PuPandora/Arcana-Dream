using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperMenu : LobbyUI
{
    private RectTransform devMenuGroupRect;
    Button[] debugButtons;

    void Awake()
    {
        devMenuGroupRect = GetComponentsInParent<RectTransform>()[1];
        debugButtons = GetComponentsInChildren<Button>();
    }

    void Start()
    {
        UIManager.instance.devMenu = this;
        UIManager.instance.devMenuGroup = devMenuGroupRect.gameObject;

        InitializeButtons();
        devMenuGroupRect.gameObject.SetActive(false);
    }

    protected override void Initialize()
    {
        UIManager.instance.devMenu = this;
        UIManager.instance.devMenuGroup = GetComponentsInParent<Transform>()[1].gameObject;
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
        InitializeButton(closeButton, () => devMenuGroupRect.gameObject.SetActive(false), "X");
    }

    private void InitializeButton(Button button, UnityEngine.Events.UnityAction call, string text)
    {
        button.onClick.AddListener(call);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        button.interactable = true;
    }

    public override void ShowUI()
    {
        devMenuGroupRect.localScale = Vector3.one;
        isOpen = true;
        devMenuGroupRect.gameObject.SetActive(true);
    }

    public override void HideUI()
    {
        devMenuGroupRect.localScale = Vector3.zero;
        isOpen = false;
        devMenuGroupRect.gameObject.SetActive(false);
    }
}
