using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public SpriteRenderer spriter { get; private set; }
    public ExpItemData data { get; private set; }

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
    }

    public void Initialize(ExpItemData data)
    {
        this.data = data;
        spriter.sprite = data.sprite;
    }
}
