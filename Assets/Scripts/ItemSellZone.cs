using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSellZone : MonoBehaviour
{
    public bool isPlayerIn;
    public bool isShopOpen;

    void Update()
    {
        if (!isPlayerIn) return;

        if (Input.GetKeyDown(GameManager.instance.interactKey))
        {
            if (isShopOpen)
            {
                EnterShop();
            }
            else
            {
                ExitShop();
            }
        }
    }

    private void EnterShop()
    {
        Debug.Log("플레이어 상점 입장");
        UIManager.instance.UIManage(UIManager.instance.inventoryUI);
        isShopOpen = true;
        GameManager.instance.playerState = PlayerState.Shop;
    }

    private void ExitShop()
    {
        Debug.Log("플레이어 상점 퇴장");
        UIManager.instance.UIManage(UIManager.instance.inventoryUI);
        isShopOpen = false;
        GameManager.instance.playerState = PlayerState.None;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            if (isShopOpen)
            {
                ExitShop();
            }
        }
    }
}
