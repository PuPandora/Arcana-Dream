using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateUpgradeZone : Zone
{
    protected override void Enter()
    {
        Debug.Log("플레이어 훈련소 입장");
        if (UIManager.instance.stateUpgradeUI == null)
        {
            Debug.LogError($"UI Manager에 stateUpgradeUI가 없습니다.");
            return;
        }

        UIManager.instance.UIManage(UIManager.instance.stateUpgradeUI);
        isUsing = true;
        GameManager.instance.playerState = PlayerState.Shop;
    }

    protected override void Exit()
    {
        Debug.Log("플레이어 훈련소 퇴장");
        UIManager.instance.UIManage(UIManager.instance.stateUpgradeUI);
        isUsing = false;
        GameManager.instance.playerState = PlayerState.None;
    }
}
