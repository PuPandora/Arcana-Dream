using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public abstract class Zone : MonoBehaviour, IInteractable
{
    public bool isOpen = true;
    protected bool isPlayerIn;
    protected bool isUsing;

    public void Interact()
    {
        if (!isUsing)
        {
            Enter();
        }
        else
        {
            Exit();
        }
    }

    public abstract void Enter();

    public abstract void Exit();

    void Update()
    {
        if (!isPlayerIn) return;

        if (Input.GetKeyDown(GameManager.instance.interactKey) && isOpen)
        {
            Interact();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            if (isUsing)
            {
                Exit();
            }
        }
    }
}
