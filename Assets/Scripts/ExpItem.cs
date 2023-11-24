using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public short expVaule;

    SpriteRenderer spriter;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    public void Initalize(ExpItemData data)
    {
        spriter.sprite = data.sprite;
        expVaule = data.value;
    }
}
