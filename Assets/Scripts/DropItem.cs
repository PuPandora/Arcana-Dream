using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public ItemDropTable[] itemDropTable;
}

[System.Serializable]
public class ItemDropTable
{
    public ItemData itemData;
    public float itemDropRate;
}