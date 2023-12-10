using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;

    [Title("Talk UI")]
    public GameObject talkPanelGroup;
    public Portrait portrait;
    public TextMeshProUGUI speakerkName;
    public TextMeshProUGUI speakerDesc;
    public TextMeshProUGUI scriptText;

    [Title("State")]
    public TalkData talkData;
    public bool isAllowTalk;
    public bool isTalking;
    public bool isScriptEnd;
    public bool isPressKey;
    public Vector3 speakerPos;

    [Title("Talk Info")]
    public byte curTalkSession;
    public byte curTalkIndex;

    [Title("Camera")]
    public CinemachineVirtualCamera zoomCam;

    [Title("Tween")]
    [SerializeField] DOTweenAnimation panelTween;
    [SerializeField] DOTweenAnimation portraitTween;

    // Event
    public event Action OnTalkStart;
    public event Action OnTalkEnd;

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
        waitUntilPress = new WaitUntil(() => isPressKey);
        OnTalkStart += (() => GameManager.instance.ChangePlayerState(PlayerState.Talk));
        OnTalkEnd += (() => GameManager.instance.ChangePlayerState(PlayerState.None));

        OnTalkStart += (() => GameManager.instance.player.SetCanMove(false));
        OnTalkEnd += (() => GameManager.instance.player.SetCanMove(true));
    }

    public void StartTalk(byte talkSessionId = 0)
    {
        // Tweening
        panelTween.DORestartById("Show");

        StartCoroutine(Talk(talkSessionId));
    }

    // To do
    // 대화 출력 중 상호작용 키를 누르면 해당 스크립트 한 번에 출력
    public IEnumerator Talk(byte talkSessionId)
    {
        isTalking = true;
        isAllowTalk = false;
        curTalkSession = talkSessionId;
        curTalkIndex = 0;
        OnTalkStart?.Invoke();

        for (int i = 0; i < talkData.scriptSession[curTalkSession].scriptData.Length; i++)
        {
            // 초기화
            scriptText.text = string.Empty;
            isScriptEnd = false;
            isPressKey = false;

            // Talk Data 불러오기
            ScriptData scriptData = talkData.GetScriptData(curTalkSession, curTalkIndex);
            speakerkName.text = scriptData.speakerData.speakerName;
            speakerDesc.text = scriptData.speakerData.spearkDesc;
            string talKScript = scriptData.script;

            // 초상화 트윈
            portraitTween.DORestartById("0");
            portrait.image.sprite = scriptData.speakerData.portraits[scriptData.spriteIndex];

            // 스크립트 한 글자씩 출력
            for (int j = 0; j < talKScript.Length; j++)
            {
                // 대화 스킵
                if (isPressKey)
                {
                    scriptText.text = talKScript;
                    break;
                }
                scriptText.text += talKScript[j];
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Talk);
                yield return Utils.delay0_05;
            }

            isScriptEnd = true;
            isPressKey = false;
            curTalkIndex++;
            yield return Utils.delay0_25;

            yield return waitUntilPress;
        }

        panelTween.DORestartById("Hide");
        OnTalkEnd?.Invoke();
        isTalking = false;

        yield return Utils.delay1;
        isAllowTalk = true;
    }
}
