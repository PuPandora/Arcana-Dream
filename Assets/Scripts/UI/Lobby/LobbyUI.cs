using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LobbyUI : MonoBehaviour
{
    protected RectTransform rect;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public bool isOpen;
    protected abstract void Initialize();

    public virtual void ShowUI()
    {
        rect.localScale = Vector3.one;
        gameObject.SetActive(true);
        isOpen = true;
    }

    public virtual void HideUI()
    {
        rect.localScale = Vector3.zero;
        gameObject.SetActive(false);
        isOpen = false;
    }
}