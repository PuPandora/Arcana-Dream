using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemName", menuName = "Scriptable Object/Item Data")]
public class ItemData : ScriptableObject
{
    [Title("Info")]
    [PreviewField(50, ObjectFieldAlignment.Center)]
    public Sprite sprite;
    public new string name;
    public string description;
    public int value;
    [Title("Stack")]
    public byte stack;
    public byte maxStack = 99;
    [Title("Advance Info")]
    public short id;
}
