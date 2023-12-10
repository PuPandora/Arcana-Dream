using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalZone : Zone
{
    private PortalRunChar[] runChars;

    public float glowDuration = 1f;
    public Ease glowEase = Ease.Linear;

    void Awake()
    {
        runChars = GetComponentsInChildren<PortalRunChar>();
    }

    public override void Interact()
    {
        UIManager.instance.UIManage(UIManager.instance.stageSelectUI);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
            Enter();

            foreach (var run in runChars)
            {
                run.OnGlow();
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!isOpen) return;

        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            Exit();

            foreach (var run in runChars)
            {
                run.OffGlow();
            }
        }
    }
}
