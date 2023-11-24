using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOrderTest : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingLayerName = "Midground";
        renderer.sortingOrder = 0;
    }
}
