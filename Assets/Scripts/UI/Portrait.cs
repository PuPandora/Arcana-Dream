using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public Image image;
    Material mat;

    void Awake()
    {
        image = GetComponent<Image>();
        mat = new Material(image.material);
    }

    public void SetColor(Color bodyColor, Color eyeColor)
    {
        mat.SetColor("_Color", bodyColor);
        mat.SetColor("_EyeColor", eyeColor);
        image.material = mat;
    }
}
