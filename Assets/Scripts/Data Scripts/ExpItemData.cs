using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpItem", menuName = "Scriptable Object/Exp Item Data")]
public class ExpItemData : ScriptableObject
{
    [PreviewField]
    public Sprite sprite;
    public short value;
}
