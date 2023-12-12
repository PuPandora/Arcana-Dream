using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSort : MonoBehaviour
{
    void Awake()
    {
        //GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }
}
