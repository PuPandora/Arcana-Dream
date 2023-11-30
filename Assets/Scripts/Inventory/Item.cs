using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : MonoBehaviour
{
    SpriteRenderer spriter;
    public ItemData itemData;
    public byte stack;
    public short id;

    // 로비 아이템 테스트용
    public bool isTestItem;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();

        if (isTestItem)
        {
            Initialize(itemData);
        }
    }

    public void Initialize(ItemData data, byte amount = 1)
    {
        itemData = data;
        spriter.sprite = data.sprite;
        id = data.id;

        // 추후 2 이상 스택 드랍도 가능하도록 기능 추가 예정
        stack = amount;

    }
}