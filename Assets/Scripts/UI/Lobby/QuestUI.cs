using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : LobbyUI
{
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        UIManager.instance.questUI = this;
        HideUI();
    }
}
