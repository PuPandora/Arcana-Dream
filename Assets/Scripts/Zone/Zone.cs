using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public abstract class Zone : MonoBehaviour, IInteractable
{
    public bool isOpen = true;
    protected bool isPlayerIn;
    protected bool isUsing;

    public abstract void Interact();

    public void Enter()
    {
        GameManager.instance.player.currentZone = this;
    }

    public void Exit()
    {
        GameManager.instance.player.currentZone = null;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
            Enter();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            Exit();
        }
    }
}
