using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Memo
// 차라리 Talk 가능한 NPC 전용 컴포넌트로 분리하는게 좋을지도?

public class TalkZone : Zone
{
    [SerializeField]
    NPC npc;

    public bool tryInteract;

    public Collider2D zone { get; private set; }

    private Coroutine tryInteractRoutine;
    private static WaitUntil waitUntilcloseUI;

    void Awake()
    {
        zone = GetComponent<Collider2D>();
        zone.enabled = npc.canTalk;
        waitUntilcloseUI = new WaitUntil(() => UIManager.instance.curUI == null);
    }

    public override void Interact()
    {
        Debug.Log($"{npc.name}에게 상호작용");
        if (tryInteractRoutine != null)
        {
            StopCoroutine(tryInteractRoutine);
        }
        tryInteractRoutine = StartCoroutine(TryInteractRoutine());

        // 대화 기능이 없는 NPC
        if (npc.talkData == null)
        {
            Debug.Log("Talk Data가 없습니다");
            return;
        }

        if (!TalkManager.instance.isAllowTalk || !npc.canTalk)
            return;

        zone.enabled = false;

        TalkStart();
    }

    // 플레이어가 상호작용을 시도했는지 여부를 확인, 일정 시간 후 다시 되돌림
    private IEnumerator TryInteractRoutine()
    {
        tryInteract = true;

        yield return Utils.delay0_1;

        tryInteract = false;
    }

    public void TalkStart(byte talkSessionId = 0)
    {
        npc.canTalk = false;
        npc.originalDirection = npc.spriter.flipX;
        npc.LookAtTarget();

        // 카메라 확대
        Vector2 playerPos = GameManager.instance.player.transform.position;
        Vector2 myPos = transform.position;

        var cam = TalkManager.instance.zoomCam.transform;

        // 플레이어와 대화 NPC 사이
        cam.position = (playerPos + myPos) * 0.5f;
        // 위치 조정, 카메라가 약간 아래로 가게
        cam.position = new Vector3(cam.position.x, cam.position.y - 1f, -10);
        TalkManager.instance.zoomCam.enabled = true;

        // TalkManager 초기화
        TalkManager.instance.talkData = npc.talkData;
        TalkManager.instance.speakerPos = npc.transform.position;
        TalkManager.instance.StartTalk(talkSessionId);
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
            npc.canTalk = true;
            StartCoroutine(npc.ResetLook());
        }

        TalkManager.instance.OnTalkEnd -= TalkEnd;
    }

    private IEnumerator ShopRoutine()
    {
        UIManager.instance.UIManage(npc.shopUI);
        GameManager.instance.playerState = PlayerState.Shop;

        yield return waitUntilcloseUI;

        if (npc.talkData.scriptSession[1] != null)
        {
            TalkManager.instance.StartTalk(1);
        }

        yield return new WaitWhile(() => TalkManager.instance.isTalking);

        GameManager.instance.playerState = PlayerState.None;
        GameManager.instance.player.canMove = true;
        TalkManager.instance.zoomCam.enabled = false;
        zone.enabled = true;
        StartCoroutine(npc.ResetLook());
        TalkManager.instance.OnTalkEnd -= TalkEnd;

        yield return Utils.delay1;

        npc.canTalk = true;
    }
}
