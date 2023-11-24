using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Title("Inventory")]
    [TableList]
    public ItemInfo[] inventory = new ItemInfo[30];

    void Start()
    {
        inventory = new ItemInfo[30];
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

        ItemInfo instantItemInfo = item.Clone();
        inventory[index] = instantItemInfo;
        inventory[index].isEmpty = false;

        return true;
    }

    public void RemoveItem(ItemInfo item)
    {

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
                Debug.Log($"인벤토리 빈 슬롯 : {i}");
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
            if (inventory[i].id != item.id) continue;

            // MaxStack 체크
            if (inventory[i].stack >= inventory[i].maxStack) continue;

            return i;
        }

        return -1;
    }
}
