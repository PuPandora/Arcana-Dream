using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : LobbyUI
{
    public Button gameResume;
    public Button gameSave;
    public Button gameOption;
    public Button mainMenu;

    protected override void Initialize()
    {
        UIManager.instance.menuUI = this;
        HideUI();

        gameResume.onClick.AddListener(GameManager.instance.Resume);
        gameSave.onClick.AddListener(GameManager.instance.SaveGame);
        //gameOption
        mainMenu.onClick.AddListener(GameManager.instance.EnterMainMenu);
    }
}
