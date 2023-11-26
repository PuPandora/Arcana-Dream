using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TextSortOrder : MonoBehaviour
{
    public enum TextType { None, DamageText}
    [EnumToggleButtons]
    public TextType type;

    Renderer m_renderer;

    void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        switch (type)
        {
            case TextType.None:
                Debug.LogWarning($"텍스트 타입이 지정되지 않은 오브젝트 : {gameObject.name}", gameObject);
                m_renderer.sortingLayerName = "Midground";
                m_renderer.sortingOrder = 0;
                break;
            case TextType.DamageText:
                m_renderer.sortingLayerName = "Foreground";
                m_renderer.sortingOrder = 3;
                break;
        }
    }
}
