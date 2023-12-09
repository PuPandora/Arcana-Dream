using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSellZone : Zone
{
    public override void Interact()
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
}
