using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ItemType : byte { Exp, Item }

public class Item : MonoBehaviour
{
    [EnumToggleButtons]
    public ItemType type;
    public short value;

    protected SpriteRenderer spriter;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }
}