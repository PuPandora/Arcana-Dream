using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalZone : Zone
{
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
}
