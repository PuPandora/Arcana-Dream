using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Memo
// 차라리 Talk 가능한 NPC 전용 컴포넌트로 분리하는게 좋을지도?

public class TalkZone : Zone
{
    [SerializeField]
    NPC npc;

    public Collider2D zone { get; private set; }

    private static WaitUntil waitUntilcloseUI;

    void Awake()
    {
        zone = GetComponent<Collider2D>();
        zone.enabled = npc.canTalk;
        waitUntilcloseUI = new WaitUntil(() => UIManager.instance.curUI == null);
    }

    public override void Interact()
    {
        // 대화 기능이 없는 NPC
        if (npc.talkData == null)
        {
            Debug.Log("Talk Data가 없습니다");
            return;
        }

        if (!TalkManager.instance.isAllowTalk)
            return;

        zone.enabled = false;

        TalkStart();
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

        // Portarit 셰이더 초기화
        var bodyColor = npc.spriter.sharedMaterial.GetColor("_Color");
        var eyeColor = npc.spriter.sharedMaterial.GetColor("_EyeColor");
        var darkColor = npc.spriter.sharedMaterial.GetFloat("_Dark");
        TalkManager.instance.portrait.SetColor(bodyColor, darkColor, eyeColor);

        // TalkManager 초기화
        TalkManager.instance.talkData = npc.talkData;
        TalkManager.instance.speakerPos = npc.transform.position;
        TalkManager.instance.StartTalk();
        TalkManager.instance.OnTalkEnd += TalkEnd;
    }

    private void TalkEnd()
    {
        if (npc.isShopNpc)
        {
            StartCoroutine(ShopRoutine());
        }
        else
        {
            TalkManager.instance.zoomCam.enabled = false;
            zone.enabled = true;
            StartCoroutine(npc.ResetLook());
            TalkManager.instance.OnTalkEnd -= TalkEnd;
        }
    }

    private IEnumerator ShopRoutine()
    {
        GameManager.instance.player.canMove = false;
        UIManager.instance.UIManage(npc.shopUI);

        yield return waitUntilcloseUI;

        GameManager.instance.player.canMove = true;
        TalkManager.instance.zoomCam.enabled = false;
        zone.enabled = true;
        StartCoroutine(npc.ResetLook());
        TalkManager.instance.OnTalkEnd -= TalkEnd;
    }
}
