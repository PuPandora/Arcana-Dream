using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private InventorySlotUI[] slots;

    void Awake()
    {
        slots = GetComponentsInChildren<InventorySlotUI>();
    }

    void OnEnable()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ApplyInventoryData(GameManager.instance.inventory.inventory[i]);
        }
    }
}
