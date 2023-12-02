using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSellZone : Zone
{
    protected override void Enter()
    {
        Debug.Log("플레이어 상점 입장");
        UIManager.instance.UIManage(UIManager.instance.inventoryUI);
        isUsing = true;
        GameManager.instance.playerState = PlayerState.Shop;
    }

    protected override void Exit()
    {
        Debug.Log("플레이어 상점 퇴장");
        UIManager.instance.UIManage(UIManager.instance.inventoryUI);
        isUsing = false;
        GameManager.instance.playerState = PlayerState.None;
    }
}
