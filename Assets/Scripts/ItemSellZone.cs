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
            // 상점 UI 오픈
            Debug.Log("상점 UI 오픈");
            GameManager.instance.inventoryUI.ShowUI();
            isShopOpen = true;
            GameManager.instance.playerState = PlayerState.Shop;
        }
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
                GameManager.instance.inventoryUI.HideUI();
                isShopOpen = false;
            }
        }
    }
}
