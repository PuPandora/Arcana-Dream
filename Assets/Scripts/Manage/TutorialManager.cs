using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    [SerializeField] TalkData tutorialData;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] private bool isPressKey;
    [SerializeField] DOTweenAnimation firstPanelTween;
    [SerializeField] DOTweenAnimation keyPanelTween;
    [SerializeField] Player player;
    [SerializeField] ExpItemData expItemData;
    [field: SerializeField]
    public StageData tutorialStageData { get; private set; }

    WaitUntil waitUntilPress;

    void Awake()
    {
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

        waitUntilPress = new WaitUntil(() => isPressKey);
    }

    void Start()
    {
        StageManager.instance.player.canMove = false;
        AudioManager.instance.PlayBgm(AudioManager.Bgm.WhiteNoise);

        StartCoroutine(TutorialRoutine());
        StartCoroutine(ShowHUDRoutine());
    }

    private IEnumerator ShowHUDRoutine()
    {
        // EXP 아이템을 먹었을 때
        yield return new WaitUntil(() => StageManager.instance.curExp != 0);

        StageManager.instance.hud.ShowTween();
    }

    // 검은 화면
    // 텍스트만 출력됨 (Tutorial Talk Data)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isPressKey = true;
        }
    }

    private IEnumerator TutorialRoutine()
    {
        player.canMove = false;
        tutorialText.text = string.Empty;
        player.AnimationDie();

        yield return EnterTutorial();
        yield return Utils.delay2;

        yield return KeyInputTutorial();
        yield return Utils.delay2;

        yield return LevelUpTutorial();
        yield return Utils.delay2;

        Debug.Log("튜토리얼 끝");

        // 보석을 먹어 레벨업 튜토리얼 루틴
        // 투척 카드 하나만 선택 가능
    }

    private IEnumerator EnterTutorial()
    {
        Debug.Log("1. 2초간 대기 중...");
        yield return Utils.delay2;

        Debug.Log("1. 튜토리얼 텍스트 시작");

        for (byte i = 0; i < tutorialData.talkSession0.Length; i++)
        {
            // 초기화
            tutorialText.text = string.Empty;
            string script = tutorialData.talkSession0[i].script;

            // 스크립트 출력
            for (short j = 0; j < tutorialData.talkSession0[i].script.Length; j++)
            {
                tutorialText.text += tutorialData.talkSession0[i].script[j];
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Talk);
                yield return Utils.delay0_05;
            }

            // 키입력 대기
            isPressKey = false;
            yield return waitUntilPress;
        }

        // 마지막 스크립트까지 다 출력했다면
        firstPanelTween.DOPlayById("Hide");

        // 독백 대화창 페이드 대기
        yield return new WaitForSeconds(firstPanelTween.duration);

        // 브금 변경
        StartCoroutine(AudioManager.instance.ChangeBgmFade(tutorialStageData.bgm));
        Debug.Log("브금 체인지");

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
        List<ExpItem> expItems = new List<ExpItem>();
        for (int i = 0; i < 30; i++)
        {
            float angle = i * Mathf.PI * 2 / 30;

            float x = Mathf.Cos(angle) * 5 + playerPos.x;
            float y = Mathf.Sin(angle) * 5 + playerPos.y;

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

            yield return Utils.delay0_05;
        }
        #endregion

        // 아이템을 먹어 1레벨 이상이 됐을 때까지 대기
        yield return new WaitUntil(() => StageManager.instance.level >= 1);

        StageManager.instance.isPlaying = true;
    }
}
