using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private RectTransform rect;
    private InventorySlotUI[] slots;
    private Inventory inventory;
    public bool isUIOpen { get; private set; }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        slots = GetComponentsInChildren<InventorySlotUI>();
        GameManager.instance.inventoryUI = this;
        isUIOpen = false;
    }

    void Start()
    {
        inventory = GameManager.instance.inventory;
    }

    [ContextMenu("Update Inventory")]
    public void UpdateInventory()
    {
        Debug.Log("Udate Inventory", gameObject);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ApplyInventoryData(inventory.inventory[i]);
        }
    }

    public void ShowUI()
    {
        UpdateInventory();
        rect.localScale = Vector3.one;
        isUIOpen = true;
    }

    public void HideUI()
    {
        rect.localScale = Vector3.zero;
        isUIOpen = false;
    }
}
