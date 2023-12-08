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

    public void SetColor(Color bodyColor, float darkColor, Color eyeColor)
    {
        mat.SetColor("_Color", bodyColor);
        mat.SetFloat("_Dark", darkColor);
        mat.SetColor("_EyeColor", eyeColor);
        image.material = mat;
    }
}
