using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesUI : LobbyUI
{
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        UIManager.instance.playerStateUI = this;
        HideUI();
    }
}
