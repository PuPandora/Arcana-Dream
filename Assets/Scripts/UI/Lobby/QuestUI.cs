using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : LobbyUI
{
    protected override void Initialize()
    {
        UIManager.instance.questUI = this;
        HideUI();
    }
}
