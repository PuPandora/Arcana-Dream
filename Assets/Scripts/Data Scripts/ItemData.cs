using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemName", menuName = "Scriptable Object/Item Data")]
public class ItemData : GameObjectData
{
    [Title("Info")]
    [PreviewField(50, ObjectFieldAlignment.Center)]
    public Sprite sprite;
    public new string name;
    public string description;
    public int value;
    public byte maxStack = 99;
}
