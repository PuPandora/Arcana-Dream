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

    public override void Enter()
    {
        Debug.Log("플레이어 맵 선택 장소 입장");
        if (UIManager.instance.stageSelectUI == null)
        {
            Debug.LogError($"UI Manager에 stageSelectUI가 없습니다.");
            return;
        }

        UIManager.instance.UIManage(UIManager.instance.stageSelectUI);
        isUsing = true;
        GameManager.instance.playerState = PlayerState.SelectMap;
    }

    public override void Exit()
    {
        Debug.Log("플레이어 맵 선택 장소 퇴장");
        UIManager.instance.UIManage(UIManager.instance.stageSelectUI);
        isUsing = false;
        GameManager.instance.playerState = PlayerState.None;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;

            foreach (var run in runChars)
            {
                run.OnGlow();
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;

            foreach (var run in runChars)
            {
                run.OffGlow();
            }
        }
    }
}
