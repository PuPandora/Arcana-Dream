using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public abstract class Zone : MonoBehaviour, IInteractable
{
    public bool isOpen = true;
    public bool isPlayerIn;
    protected bool isUsing;
    public bool isDisplayKeyGuide = true;
    public string keyInputText;

    public abstract void Interact();

    public virtual void Enter()
    {
        GameManager.instance.player.currentZone = this;

        if (isDisplayKeyGuide)
        {
            LobbyManager.instance.keyInputGuide.SetGuideText(keyInputText);
            LobbyManager.instance.keyInputGuide.Show();
        }
    }

    public virtual void Exit()
    {
        GameManager.instance.player.currentZone = null;

        if (isDisplayKeyGuide)
        {
            LobbyManager.instance.keyInputGuide.Hide();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
            Enter();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!isOpen) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            Exit();
        }
    }
}
