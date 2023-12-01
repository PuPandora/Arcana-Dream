using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LobbyUI : MonoBehaviour
{
    public bool isOpen;
    protected abstract void Initialize();
    public abstract void ShowUI();
    public abstract void HideUI();
}