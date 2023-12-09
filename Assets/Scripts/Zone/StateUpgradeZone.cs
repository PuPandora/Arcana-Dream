using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUpgradeZone : Zone
{
    public override void Interact()
    {
        if (UIManager.instance.stateUpgradeUI == null)
        {
            Debug.LogError($"UI Manager에 stateUpgradeUI가 없습니다.");
            return;
        }

        UIManager.instance.UIManage(UIManager.instance.stateUpgradeUI);
        GameManager.instance.playerState = PlayerState.Shop;
    }
}
