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
        GameManager.instance.inventoryUI.ShowUI();
        isShopOpen = true;
        GameManager.instance.playerState = PlayerState.Shop;
    }

    private void ExitShop()
    {
        Debug.Log("플레이어 상점 퇴장");
        GameManager.instance.inventoryUI.HideUI();
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
