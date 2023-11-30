using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public short id;
    public byte stack = 0;
    public bool isEmpty = true;

    public void AddStack(byte amount)
    {
        // MaxStack 체크

        stack += amount;
    }

    public void RemoveStack(byte amount)
    {
        // MaxStack 체크

        stack -= amount;
    }
}

public class Inventory : MonoBehaviour
{
    [Title("Inventory")]
    [TableList]
    public InventorySlot[] inventory = new InventorySlot[30];

    void Awake()
    {
        ClearInventory();
        GameManager.instance.inventory = this;
    }

    public bool AddItem(Item item, byte amount = 1)
    {
        sbyte index = 0;
        index = CheckAlreadyHaveItem(item);

        if (index != -1)
        {
            inventory[index].AddStack(amount);
            return true;
        }

        index = CheckEmptySlot();
        if (index == -1) return false;

        inventory[index].itemData = item.selfData;
        inventory[index].id = item.selfData.id;
        inventory[index].isEmpty = false;

        return true;
    }

    public void RemoveItem(byte index, byte count = 1)
    {
        if (inventory[index].stack >= count)
        {
            inventory[index].stack -= count;
        }
    }

    /// <summary>
    /// 인벤토리의 빈 슬롯 인덱스를 반환합니다.
    /// </summary>
    /// <returns>빈 슬롯이 있다면 해당 위치 인덱스를 반환. <br></br> 없다면 -1을 반환합니다.</returns>
    private sbyte CheckEmptySlot()
    {
        for (sbyte i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].isEmpty)
            {
                return i;
            }
        }

        Debug.Log("인벤토리에 빈 슬롯이 없습니다.");
        return -1;
    }

    /// <summary>
    /// 인벤토리에 이미 해당 아이템이 있는지 검사합니다
    /// </summary>
    /// <param name="item"></param>
    /// <returns>동일한 아이템이 있다면 해당 위치 인덱스를 반환. <br></br> 없다면 -1을 반환합니다.</returns>
    private sbyte CheckAlreadyHaveItem(Item item)
    {
        for (sbyte i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].isEmpty) continue;

            // id 체크
            if (inventory[i].itemData.id != item.id) continue;

            // MaxStack 체크
            if (inventory[i].stack >= inventory[i].itemData.maxStack) continue;

            return i;
        }

        return -1;
    }

    [ContextMenu("Refresh Data")]
    public void RefreshData()
    {
        foreach (var item in inventory)
        {
            if (item.isEmpty) continue;

            item.itemData = Utils.GetItemDataWithId(item.id);
        }
    }

    public void ClearInventory()
    {
        Debug.Log("인벤토리 초기화", gameObject);
        inventory = new InventorySlot[30];
        for (byte i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new InventorySlot();
        }
    }
}
