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
    public byte stack
    {
        get
        {
            return m_stack;
        }
        set
        {
            m_stack = value;

            if (m_stack <= 0)
            {
                Clear();
            }
        }
    }
    [SerializeField] private byte m_stack;
    public bool isEmpty = true;

    private void Clear()
    {
        itemData = null;
        id = 0;
        m_stack = 0;
        isEmpty = true;
    }
}

public class Inventory : MonoBehaviour
{
    [Title("Inventory")]
    [TableList]
    public InventorySlot[] inventory = new InventorySlot[Utils.INVENTORY_SLOT_COUNT];

    /// <summary>
    /// 인벤토리에 아이템을 추가합니다.
    /// </summary>
    /// <returns>추가됐다면 true, 추가하지 못했다면 false를 반환합니다.</returns>
    public bool AddItem(Item item, byte amount = 1)
    {
        byte? index = CheckAlreadyHaveItem(item);

        // 이미 동일한 아이템이 있을 경우
        if (index.HasValue)
        {
            inventory[index.Value].stack += amount;
            if (UIManager.instance.inventoryUI != null)
            {
                UIManager.instance.inventoryUI.UpdateInventory(index.Value);
            }
            return true;
        }

        // 들어갈 수 있는 공간이 있는지
        index = CheckEmptySlot();
        if (!index.HasValue) return false;

        // 인벤토리 배열[index]를 받은 아이템 데이터로 초기화
        inventory[index.Value].itemData = item.data;
        inventory[index.Value].id = item.data.id;
        inventory[index.Value].isEmpty = false;
        inventory[index.Value].stack += amount;

        // 인벤토리 UI가 있는 경우
        // 만약 아이템이 추가되는 모든 씬에 UIManager와 인벤토리 UI가 있다면 스킵 가능
        if (UIManager.instance && UIManager.instance.inventoryUI)
        {
            UIManager.instance.inventoryUI.UpdateInventory(index.Value);
        }

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
    /// <returns>빈 슬롯이 있다면 해당 위치 인덱스를 반환. <br></br> 없다면 null을 반환합니다.</returns>
    private byte? CheckEmptySlot()
    {
        for (byte i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].isEmpty)
            {
                return i;
            }
        }

        Debug.Log("인벤토리에 빈 슬롯이 없습니다.");
        return null;
    }

    /// <summary>
    /// 인벤토리에 이미 해당 아이템이 있는지 검사합니다
    /// </summary>
    /// <returns>동일한 아이템이 있다면 해당 위치 인덱스를 반환. <br></br> 없다면 null을 반환합니다.</returns>
    private byte? CheckAlreadyHaveItem(Item item)
    {
        for (byte i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].isEmpty) continue;

            // id 체크
            if (inventory[i].itemData.id != item.data.id) continue;

            // MaxStack 체크
            if (inventory[i].stack >= inventory[i].itemData.maxStack) continue;

            return i;
        }

        return null;
    }

    /// <summary>
    /// 인벤토리의 ItemData를 새로고침합니다.
    /// </summary>
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

        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new InventorySlot();
        }
    }

    /// <summary>
    /// 현재 인벤토리 정보를 저장 데이터로 내보내는 메소드
    /// </summary>
    /// <returns></returns>
    public InventoryData GetInventoryData()
    {
        var result = new InventoryData();

        for (int i = 0; i < result.count; i++)
        {
            if (inventory[i].isEmpty) continue;

            result.itemIds[i] = inventory[i].itemData.id;
            result.stacks[i] = inventory[i].stack;
        }

        return result;
    }

    /// <summary>
    /// 저장된 인벤토리 데이터를 불러와 적용시키는 메소드
    /// </summary>
    /// <param name="data"></param>
    public void ApplyData(InventoryData data)
    {
        for (int i = 0; i < data.count; i++)
        {
            // 아이템이 없는 경우 넘어감
            if (data.itemIds[i] == 0) continue;

            inventory[i].isEmpty = false;
            inventory[i].id = data.itemIds[i];
            inventory[i].stack = data.stacks[i];
        }

        RefreshData();
    }

    public void SellItem(byte index)
    {
        if (inventory[index].isEmpty)
        {
            UIManager.instance.inventoryUI.UpdateInventory(index);
            return;
        }

        GameManager.instance.SetGold(GameManager.instance.gold + inventory[index].itemData.value);
        Debug.Log($"{inventory[index].itemData.name} 아이템 판매.\n" +
            $"얻은 골드 : {inventory[index].itemData.value}");
        RemoveItem(index);

        UIManager.instance.inventoryUI.UpdateInventory(index);
    }
}
