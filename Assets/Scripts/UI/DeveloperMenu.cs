using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperMenu : MonoBehaviour
{
    private RectTransform devMenuGroupRect;
    Button[] debugButtons;

    void Awake()
    {
        devMenuGroupRect = GetComponentsInParent<RectTransform>()[1];
        debugButtons = GetComponentsInChildren<Button>();

        HideUI();
    }

    void Start()
    {
        GameManager.instance.devMenu = this;
        GameManager.instance.devMenuGroup = devMenuGroupRect.gameObject;

        InitializeButtons();

        devMenuGroupRect.gameObject.SetActive(false);
    }

    private void InitializeButtons()
    {
        foreach (var button in debugButtons)
        {
            button.interactable = false;
        }

        var inventoryClearBtn = debugButtons[0];
        var saveGameBtn = debugButtons[1];
        var loadGameBtn = debugButtons[2];
        var lobbyBtn = debugButtons[3];

        inventoryClearBtn.onClick.AddListener(GameManager.instance.ClearInventory);
        inventoryClearBtn.GetComponentInChildren<TextMeshProUGUI>().text = "인벤토리 초가화";
        inventoryClearBtn.interactable = true;

        saveGameBtn.onClick.AddListener(GameManager.instance.SaveGame);
        saveGameBtn.GetComponentInChildren<TextMeshProUGUI>().text = "게임 저장";
        saveGameBtn.interactable = true;

        loadGameBtn.onClick.AddListener(GameManager.instance.LoadGame);
        loadGameBtn.GetComponentInChildren<TextMeshProUGUI>().text = "게임 불러오기";
        loadGameBtn.interactable = true;

        lobbyBtn.onClick.AddListener(GameManager.instance.ExitStage);
        lobbyBtn.GetComponentInChildren<TextMeshProUGUI>().text = "로비로 이동";
        lobbyBtn.interactable = true;

        var closeButton = debugButtons[debugButtons.Length - 1];
        Debug.Log(closeButton.name, closeButton.gameObject);
        closeButton.onClick.AddListener(() => devMenuGroupRect.gameObject.SetActive(false));
        closeButton.GetComponentInChildren<TextMeshProUGUI>().text = "X";
        closeButton.interactable = true;
    }

    public void ShowUI()
    {
        devMenuGroupRect.localScale = Vector3.one;
    }

    public void HideUI()
    {
        devMenuGroupRect.localScale = Vector3.zero;
    }
}
