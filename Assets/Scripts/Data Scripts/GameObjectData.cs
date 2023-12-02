using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameObjectData : ScriptableObject
{
    [field: SerializeField]
    public short id { get; private set; }
}