using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Item
{
    public void Initalize(ExpItemData data)
    {
        spriter.sprite = data.sprite;
        value = data.value;
    }
}
