using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Memo
// 차라리 Talk 가능한 NPC 전용 컴포넌트로 분리하는게 좋을지도?

public class TalkZone : MonoBehaviour
{
    [SerializeField] NPC npc;
    private Vector3 originalCamPos;

    void Awake()
    {
        npc = GetComponentInParent<NPC>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어만
        if (collision.transform != GameManager.instance.player.transform)
        {
            return;
        }

        if (Input.GetKey(GameManager.instance.interactKey) && TalkManager.instance.isAllowTalk)
        {
            if (npc.talkData == null)
            {
                Debug.Log("Talk Data가 없습니다");
                // 대화 기능이 없는 NPC
                return;
            }

            TalkStart();
        }
    }

    private void TalkStart()
    {
        // 카메라 확대
        Vector2 playerPos = GameManager.instance.player.transform.position;
        Vector2 myPos = transform.position;

        var cam = TalkManager.instance.zoomCam.transform;

        // 플레이어와 대화 NPC 사이
        cam.position = (playerPos + myPos) * 0.5f;
        // 위치 조정, 카메라가 약간 아래로 가게
        cam.position = new Vector3(cam.position.x, cam.position.y - 1f, -10);
        TalkManager.instance.zoomCam.enabled = true;

        // UI Manager 초기화, 대화창 열기
        var bodyColor = npc.spriter.sharedMaterial.GetColor("_Color");
        var eyeColor = npc.spriter.sharedMaterial.GetColor("_EyeColor");
        var darkColor = npc.spriter.sharedMaterial.GetFloat("_Dark");
        TalkManager.instance.portrait.SetColor(bodyColor, darkColor, eyeColor);
        TalkManager.instance.talkData = npc.talkData;
        TalkManager.instance.speakerPos = npc.transform.position;
        TalkManager.instance.ShowUI();
        TalkManager.instance.OnTalkEnd += TalkEnd;
    }

    private void TalkEnd()
    {
        TalkManager.instance.zoomCam.enabled = false;
        StartCoroutine(npc.ResetLook());
        TalkManager.instance.OnTalkEnd -= TalkEnd;
    }
}
