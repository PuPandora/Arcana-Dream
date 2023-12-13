using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public enum TutorialType : byte { FirstStage, FirstLobby }

    [ReadOnly]
    private TutorialType type;

    [Title("Actor")]
    [SerializeField] Player player;
    [SerializeField] NPC pandora;
    [SerializeField] NPC bell;
    [SerializeField] NPC kara;

    [Title("Public")]
    public Transform target;

    [Title("First Tutorial")]
    public GameObject stageTutorialGroup;
    [SerializeField] TalkData tutorialData;
    [SerializeField] TextMeshProUGUI tutorialText;
    public bool isPressKey;
    [SerializeField] DOTweenAnimation firstPanelTween;
    [SerializeField] DOTweenAnimation keyPanelTween;
    public Image[] moveKeyImages;
    [SerializeField] ExpItemData expItemData;
    public StageData tutorialStageData;
    public Button skipButton;

    [Title("Second Tutorial")]
    public Transform[] pandoraFirstPath;
    public Transform[] BellStorePath;
    public Transform[] KaraTraningPath;
    public Transform[] pandoraBackPath;
    public Transform[] placeZoomPoints;
    public TalkData tutorialPandoraTalkData;

    // Event
    public event Action OnTutorialStart;
    public event Action OnTutorialEnd;

    WaitUntil waitUntilPress;

    void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        //Debug.Log(GameManager.instance.gameState);
        //switch (GameManager.instance.gameState)
        //{

        //}
        if (StageManager.instance != null)
        {
            // 새 게임이 아닌 경우 비활성화
            if (GameManager.instance.isNewGame)
            {
                stageTutorialGroup.SetActive(true);
                stageTutorialGroup.GetComponent<RectTransform>().localScale = Vector3.one;
                skipButton.gameObject.SetActive(true);
                GameManager.instance.playerState = PlayerState.Tutorial;
                StartTutorial(TutorialType.FirstStage);
            }
            else
            {
                // MEMO
                // 새 게임이 아닌 경우 어느 쪽이 좋을까?
                // 튜토리얼 오브젝트 생성? 혹은 스테이지에 이미 있고 끄기(파괴)하기?
                firstPanelTween.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        // 임시 왜인지 튜토리얼 스테이지 진입시 Lobby로 뜸
        if (LobbyManager.instance != null && GameManager.instance.isNewGame)
        {
            //StartTutorial(TutorialType.FirstLobby);
        }

        OnTutorialStart += (() => GameManager.instance.ChangePlayerState(PlayerState.Tutorial));
        OnTutorialEnd += (() => GameManager.instance.ChangePlayerState(PlayerState.None));

        waitUntilPress = new WaitUntil(() => isPressKey);
    }

    public void StartTutorial(TutorialType type)
    {
        this.type = type;

        switch (type)
        {
            case TutorialType.FirstStage:
                StartCoroutine(FirstTutorialRoutine());
                break;
            case TutorialType.FirstLobby:
                StartCoroutine(FirstLobbyRoutine());
                break;
        }
    }

    private IEnumerator ShowHUDRoutine()
    {
        StageManager.instance.hud.canvasGroup.alpha = 0;
        StageManager.instance.hud.cameraCanvasGroup.alpha = 0;

        // EXP 아이템을 먹었을 때
        yield return new WaitUntil(() => StageManager.instance.curExp != 0);

        StageManager.instance.hud.ShowTween();
    }

    // 검은 화면
    // 텍스트만 출력됨 (Tutorial Talk Data)

    #region 첫 튜토리얼 - 스테이지
    /// <summary>
    /// 새 게임 첫 스테이지 시작 루틴
    /// </summary>
    public IEnumerator FirstTutorialRoutine()
    {
        OnTutorialStart?.Invoke();

        AudioManager.instance.PlayBgm(AudioManager.Bgm.WhiteNoise);
        StartCoroutine(ShowHUDRoutine());

        player.canMove = false;
        tutorialText.text = string.Empty;
        player.AnimationDie();

        yield return EnterTutorial();
        yield return Utils.delay2;

        yield return KeyInputTutorial();
        yield return Utils.delay2;

        yield return LevelUpTutorial();
        yield return Utils.delay2;

        Debug.Log("스테이지 튜토리얼 끝");
        OnTutorialEnd?.Invoke();

        // 2. 로비 튜토리얼
        yield return new WaitUntil(() => GameManager.instance.gameState == GameState.Lobby);

        // 보석을 먹어 레벨업 튜토리얼 루틴
        // 투척 카드 하나만 선택 가능

    }

    private IEnumerator EnterTutorial()
    {
        Debug.Log("1. 2초간 대기 중...");
        yield return Utils.delay2;

        Debug.Log("1. 튜토리얼 텍스트 시작");

        for (byte i = 0; i < tutorialData.scriptSession[0].scriptData.Length; i++)
        {
            // 초기화
            tutorialText.text = string.Empty;
            string script = tutorialData.scriptSession[0].scriptData[i].script;

            // 스크립트 출력
            for (short j = 0; j < tutorialData.scriptSession[0].scriptData[i].script.Length; j++)
            {
                tutorialText.text += tutorialData.scriptSession[0].scriptData[i].script[j];
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Talk);
                yield return Utils.delay0_05;
            }

            // 키입력 대기
            isPressKey = false;
            yield return Utils.delay0_25;

            yield return waitUntilPress;
        }

        // 마지막 스크립트까지 다 출력했다면
        firstPanelTween.DOPlayById("Hide");

        // 독백 대화창 페이드 대기
        yield return new WaitForSeconds(firstPanelTween.duration);

        // 브금 변경
        AudioManager.instance.ChangeBgmFade(tutorialStageData.bgm, 2f);

        firstPanelTween.GetComponent<Image>().raycastTarget = false;
        StageManager.instance.transition.Open();

        // 트렌지션 대기
        yield return new WaitForSeconds(StageManager.instance.transition.transitionA.duration * 0.5f);
        player.WakeUp();

        Debug.Log("1. 튜토리얼 첫 대사 끝");

        // 플레이어 애니메이션 대기
        yield return Utils.delay1;
    }

    private IEnumerator KeyInputTutorial()
    {
        bool[] moveKeyInputs = new bool[4];
        bool isPressAllMoveKey = false;
        StageManager.instance.player.canMove = true;

        Debug.Log("2. 키 튜토리얼 시작");

        // WASD 키 보여주기
        keyPanelTween.DOPlayById("Show");

        // WASD키를 모두 한 번씩 누를 때 까지
        // 플레이어 움직임이 있을 때 WASD 입력 감지
        Color keyPressedColor = new Color(0.55f, 0.55f, 0.55f, 0.6f);

        while (!isPressAllMoveKey)
        {
            if (!Mathf.Approximately(player.moveInput.magnitude, 0))
            {
                // W키
                if (player.moveInput.y > 0f)
                {
                    moveKeyInputs[0] = true;
                    moveKeyImages[0].color = keyPressedColor;
                }
                // A키
                else if (player.moveInput.x < 0f)
                {
                    moveKeyInputs[1] = true;
                    moveKeyImages[1].color = keyPressedColor;
                }
                // S키
                if (player.moveInput.y < 0f)
                {
                    moveKeyInputs[2] = true;
                    moveKeyImages[2].color = keyPressedColor;
                }
                // D키
                else if (player.moveInput.x > 0f)
                {
                    moveKeyInputs[3] = true;
                    moveKeyImages[3].color = keyPressedColor;
                }
            }

            isPressAllMoveKey = 
                moveKeyInputs[0] && 
                moveKeyInputs[1] && 
                moveKeyInputs[2] && 
                moveKeyInputs[3];

            yield return null;
        }

        yield return Utils.delay1;

        // WASD 키 숨기기
        keyPanelTween.DOPlayById("Hide");

        Debug.Log("키 입력 튜토리얼 완료");
    }

    private IEnumerator LevelUpTutorial()
    {
        #region 아이템 소환 (임시)
        Vector3 playerPos = StageManager.instance.player.transform.position;
        byte itemCount = 30;
        byte radius = 5;
        // 0.5초 동안 모든 아이템 등장
        WaitForSeconds delay = new WaitForSeconds(0.5f / itemCount);

        List<ExpItem> expItems = new List<ExpItem>();
        for (int i = 0; i < itemCount; i++)
        {
            float angle = i * Mathf.PI * 2 / itemCount;

            float x = Mathf.Cos(angle) * radius + playerPos.x;
            float y = Mathf.Sin(angle) * radius + playerPos.y;

            var item = GameManager.instance.poolManager.Get(PoolType.ExpItem);
            var expItem = item.GetComponent<ExpItem>();

            expItem.Initialize(expItemData);
            expItem.transform.position = new Vector3(x, y, 0);
            expItem.transform.rotation = Quaternion.identity;
            expItem.spriter.color = new Color(1, 1, 1, 0);

            expItems.Add(expItem);
        }
        // 아이템 Fade Tween
        foreach (var item in expItems)
        {
            item.spriter.DOFade(1, 1f)
                .SetEase(Ease.OutCubic);

            yield return delay;
        }
        #endregion

        // 아이템을 먹어 1레벨 이상이 됐을 때까지 대기
        yield return new WaitUntil(() => StageManager.instance.level >= 1);

        StageManager.instance.isPlaying = true;
    }
    #endregion

    #region 로비 튜토리얼 - 첫 만남
    private IEnumerator FirstLobbyRoutine()
    {
        player.canMove = false;

        pandora.talkZone.zone.enabled = false;
        bell.talkZone.zone.enabled = false;
        bell.canTalk = false;
        kara.talkZone.zone.enabled = false;
        kara.canTalk = false;

        yield return MeetPandoraRoutine();

        yield return MeetBellRoutine();

        yield return MeetKaraRoutine();

        yield return IntroduceEnterStageRoutine();
    }

    private IEnumerator MeetPandoraRoutine()
    {
        pandora.talkData = tutorialPandoraTalkData;
        pandora.SetCanTalk(false);

        // 판도라 이동
        pandora.Move(pandoraFirstPath[0].position);

        // 판도라 도착
        yield return new WaitWhile(() => pandora.isMoving);

        // 첫 대화, 안내
        pandora.talkZone.TalkStart();

        // 벨 상점 포커스
        yield return new WaitUntil(() => TalkManager.instance.curTalkIndex == 8);
        LobbyManager.instance.placeZoomCam.transform.position = placeZoomPoints[0].position;
        LobbyManager.instance.placeZoomCam.enabled = true;

        // 포커스 해제
        yield return new WaitWhile(() => TalkManager.instance.curTalkIndex == 8);
        LobbyManager.instance.placeZoomCam.enabled = false;

        // 대화창 닫을 때 까지 대기
        yield return new WaitWhile(() => TalkManager.instance.isTalking);
        pandora.talkZone.zone.enabled = false;

        // 판도라 벨의 상점으로 이동
        player.canMove = true;
        pandora.Move(BellStorePath[0].position);
        yield return new WaitWhile(() => pandora.isMoving);

        pandora.Move(BellStorePath[1].position);
        yield return new WaitWhile(() => pandora.isMoving);

        pandora.LookRight();
    }

    private IEnumerator MeetBellRoutine()
    {
        // 플레이어가 벨의 상점에서 상호작용 할 때 까지 대기
        bell.talkZone.zone.enabled = true;
        yield return new WaitUntil(() => bell.talkZone.tryInteract);
        bell.talkZone.zone.enabled = false;

        // 벨 소개 튜토리얼
        pandora.talkZone.TalkStart(1);
        yield return new WaitWhile(() => TalkManager.instance.isTalking);
        pandora.talkZone.zone.enabled = false;

        // 인벤토리 열기
        UIManager.instance.UIManage(UIManager.instance.inventoryUI);
        GameManager.instance.playerState = PlayerState.Shop;

        // 아이템 판매 튜토리얼
        Debug.Log("~ 이렇게 저렇게 아이템을 판매할 수 있습니다.");

        // 인벤토리 닫기
        yield return new WaitWhile(() => UIManager.instance.curUI == UIManager.instance.inventoryUI);
        GameManager.instance.playerState = PlayerState.None;
    }

    private IEnumerator MeetKaraRoutine()
    {
        // 스탯 업그레이드 튜토리얼
        pandora.talkZone.TalkStart(2);

        // 카메라 포커스
        yield return new WaitUntil(() => TalkManager.instance.curTalkIndex == 2);
        LobbyManager.instance.placeZoomCam.transform.position = placeZoomPoints[1].position;
        LobbyManager.instance.placeZoomCam.enabled = true;

        yield return new WaitUntil(() => TalkManager.instance.curTalkIndex != 2);
        LobbyManager.instance.placeZoomCam.enabled = false;

        // 대화 종료 대기
        yield return new WaitWhile(() => TalkManager.instance.isTalking);
        pandora.talkZone.zone.enabled = false;

        // 벨 상호작용 활성화
        bell.canTalk = true;
        bell.talkZone.zone.enabled = true;
        bell.talkData = LobbyManager.instance.bell0;

        // 판도라 이동
        pandora.Move(KaraTraningPath[0].position);
        yield return new WaitWhile(() => pandora.isMoving);

        pandora.Move(KaraTraningPath[1].position);
        yield return new WaitWhile(() => pandora.isMoving);

        pandora.LookRight();
        yield return new WaitWhile(() => pandora.isMoving);
        // 플레이어가 카라의 훈련장에서 상호작용 할 때 까지 대기
        kara.talkZone.zone.enabled = true;
        yield return new WaitUntil(() => kara.talkZone.tryInteract);
        kara.talkZone.zone.enabled = false;

        pandora.talkZone.TalkStart(3);

        yield return new WaitWhile(() => TalkManager.instance.isTalking);

        UIManager.instance.UIManage(UIManager.instance.stateUpgradeUI);

        yield return new WaitUntil(() => UIManager.instance.curUI == null);
    }

    private IEnumerator IntroduceEnterStageRoutine()
    {
        // 소개를 마치고 포탈로 돌아가기로 함
        pandora.talkZone.TalkStart(4);
        yield return new WaitWhile(() => TalkManager.instance.isTalking);
        pandora.talkZone.zone.enabled = false;

        // 카라 상호작용 활성화
        kara.talkZone.zone.enabled = true;
        kara.canTalk = true;
        kara.talkData = LobbyManager.instance.kara0;

        // 판도라 원래 위치로 돌아감
        pandora.Move(pandoraBackPath[0].position);
        yield return new WaitWhile(() => pandora.isMoving);

        pandora.Move(pandoraBackPath[1].position);
        yield return new WaitWhile(() => pandora.isMoving);

        pandora.LookLeft();
        pandora.talkZone.zone.enabled = true;
        pandora.canTalk = false;
        yield return new WaitUntil(() => pandora.talkZone.tryInteract);

        // 세계관 설명
        pandora.talkZone.TalkStart(5);
        pandora.talkZone.zone.enabled = false;
        yield return new WaitWhile(() => TalkManager.instance.isTalking);

        yield return Utils.delay1;

        pandora.canTalk = false;
        yield return new WaitUntil(() => pandora.talkZone.tryInteract);
        // 포탈 사용 설명
        pandora.talkZone.TalkStart(6);
        pandora.talkZone.zone.enabled = false;
        yield return new WaitWhile(() => TalkManager.instance.isTalking);

        // 판도라 상호작용 활성화
        pandora.talkZone.zone.enabled = true;
        pandora.canTalk = true;
        pandora.talkData = LobbyManager.instance.pandora0;

        // 첫 튜토리얼 종료, 자유행동 가능
        GameManager.instance.playerState = PlayerState.None;
        GameManager.instance.isNewGame = false;
        LobbyManager.instance.portal.isOpen = true;
    }
    #endregion
}
