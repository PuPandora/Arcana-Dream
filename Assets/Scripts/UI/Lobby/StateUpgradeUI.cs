using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUpgradeUI : LobbyUI
{
    protected override void Initialize()
    {
        UIManager.instance.stateUpgradeUI = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        HideUI();
    }
}
