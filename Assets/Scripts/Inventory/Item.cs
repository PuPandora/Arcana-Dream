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
}