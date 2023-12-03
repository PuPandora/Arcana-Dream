using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSellZone : Zone
{
    public override void Enter()
    {
        Debug.Log("플레이어 상점 입장");
        if (UIManager.instance.inventoryUI == null)
        {
            Debug.LogError($"UI Manager에 inventoryUI가 없습니다.");
            return;
        }

        UIManager.instance.UIManage(UIManager.instance.inventoryUI);
        isUsing = true;
        GameManager.instance.playerState = PlayerState.Shop;
    }

    public override void Exit()
    {
        Debug.Log("플레이어 상점 퇴장");
        UIManager.instance.UIManage(UIManager.instance.inventoryUI);
        isUsing = false;
        GameManager.instance.playerState = PlayerState.None;
    }
}
