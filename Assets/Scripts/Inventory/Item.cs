using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : MonoBehaviour
{
    SpriteRenderer spriter;
    public ItemData selfData;

    [Title("Info")]
    public new string name;
    public string description;
    public int value;
    [Title("Stack")]
    public byte stack;
    public byte maxStack;
    [Title("Advance Info")]
    public short id;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (selfData)
        {
            Initialize(selfData);
        }
    }

    public void Initialize(ItemData data)
    {
        spriter.sprite = data.sprite;

        name = data.name;
        description = data.description;
        value = data.value;

        //stack = data.stack;
        // Test Temp Code
        stack = 1;
        maxStack = data.maxStack;

        id = data.id;
    }

    public ItemInfo Clone()
    {
        ItemInfo item = new ItemInfo();

        item.sprite = spriter.sprite;
        item.name = name;
        item.description = description;
        item.value = value;

        item.stack = stack;
        item.maxStack = maxStack;

        item.id = id;

        return item;
    }
}

[System.Serializable]
public class ItemInfo
{
    [PreviewField(70, ObjectFieldAlignment.Center)]
    public Sprite sprite;

    [VerticalGroup("Basic Info")]
    public string name;
    [VerticalGroup("Basic Info")]
    [TextArea(3, 10)]
    public string description;

    [VerticalGroup("Advance Info")]
    public int value;
    [VerticalGroup("Advance Info")]
    public byte stack;
    [VerticalGroup("Advance Info")]
    public byte maxStack;
    [VerticalGroup("Advance Info")]
    public short id;
    [VerticalGroup("Advance Info")]
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