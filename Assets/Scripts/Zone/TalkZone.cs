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

            // 카메라 확대
            Vector2 playerPos = GameManager.instance.player.transform.position;
            Vector2 myPos = transform.position;

            var camPos = npc.zoomCam.transform.position;
            originalCamPos = camPos;

            camPos = (playerPos + myPos) * 0.5f;
            camPos = new Vector3(camPos.x, camPos.y, -10);
            npc.zoomCam.enabled = true;

            // UI Manager 초기화, 대화창 열기
            var bodyColor = npc.spriter.sharedMaterial.GetColor("_Color");
            var eyeColor = npc.spriter.sharedMaterial.GetColor("_EyeColor");
            TalkManager.instance.portrait.SetColor(bodyColor, eyeColor);
            TalkManager.instance.talkData = npc.talkData;
            TalkManager.instance.speakerPos = npc.transform.position;
            TalkManager.instance.ShowUI();
            TalkManager.instance.OnTalkEnd += TalkEnd;
        }
    }

    private void TalkEnd()
    {
        npc.zoomCam.enabled = false;
        npc.zoomCam.transform.position = originalCamPos;
        StartCoroutine(npc.ResetLook());
        TalkManager.instance.OnTalkEnd -= TalkEnd;
    }
}
