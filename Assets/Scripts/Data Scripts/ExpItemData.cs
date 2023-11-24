using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpItem", menuName = "Scriptable Object/Exp Item Data")]
public class ExpItemData : ScriptableObject
{
    public Sprite sprite;
    public short value;
}
