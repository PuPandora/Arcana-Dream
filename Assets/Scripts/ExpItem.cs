using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    SpriteRenderer spriter;
    public ExpItemData data { get; private set; }

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    public void Initalize(ExpItemData data)
    {
        this.data = data;
        spriter.sprite = data.sprite;
    }
}
