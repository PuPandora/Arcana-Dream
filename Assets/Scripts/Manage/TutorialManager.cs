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
    [SerializeField] NPC Pandora;
    [SerializeField] NPC Bell;

    [Title("First Tutorial")]
    [SerializeField] TalkData tutorialData;
    [SerializeField] TextMeshProUGUI tutorialText;
    public bool isPressKey;
    [SerializeField] DOTweenAnimation firstPanelTween;
    [SerializeField] DOTweenAnimation keyPanelTween;
    [SerializeField] ExpItemData expItemData;
    public StageData tutorialStageData;

    [Title("Second Tutorial")]

    // Event
    public event Action OnTutorialStart;
    public event Action OnTutorialEnd;

    WaitUntil waitUntilPress;

    void Awake()
    {
        // 새 게임이 아닌 경우 비활성화
        if (!GameManager.instance.isNewGame)
        {
            // MEMO
            // 새 게임이 아닌 경우 어느 쪽이 좋을까?
            // 튜토리얼 오브젝트 생성? 혹은 스테이지에 이미 있고 끄기(파괴)하기?
            firstPanelTween.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

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
        while (!isPressAllMoveKey)
        {
            if (!Mathf.Approximately(player.moveInput.magnitude, 0))
            {
                // A키
                if (player.moveInput.x > 0f)
                {
                    moveKeyInputs[0] = true;
                }
                // D키
                else if (player.moveInput.x < 0f)
                {
                    moveKeyInputs[1] = true;
                }
                // W키
                if (player.moveInput.y > 0f)
                {
                    moveKeyInputs[2] = true;
                }
                // S키
                else if (player.moveInput.y < 0f)
                {
                    moveKeyInputs[3] = true;
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

            expItem.Initalize(expItemData);
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
        yield return MeetPandoraRoutine();
    }

    private IEnumerator MeetPandoraRoutine()
    {
        //Pandora.move
        yield return null;
    }
    #endregion
}
