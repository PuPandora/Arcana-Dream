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
    // 다른 스크립트에서 대화를 제어할 때
    public bool isWait;

    [Title("Camera")]
    public CinemachineVirtualCamera zoomCam;
    public CinemachineVirtualCamera changeCam;

    [Title("Tween")]
    [SerializeField] DOTweenAnimation panelTween;
    [SerializeField] DOTweenAnimation portraitTween;

    // Event
    public event Action OnTalkStart;
    public event Action OnTalkEnd;

    WaitUntil untilPressKey;

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
        untilPressKey = new WaitUntil(() => isPressKey);

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

        SpeakerData prevSpeaker = null;

        for (int i = 0; i < talkData.scriptSession[curTalkSession].scriptData.Length; i++)
        {
            // 초기화
            scriptText.text = string.Empty;
            isScriptEnd = false;
            isPressKey = false;
            isWait = false;

            // Talk Data 불러오기
            ScriptData scriptData = talkData.GetScriptData(curTalkSession, curTalkIndex);
            speakerkName.text = scriptData.speakerData.speakerName;
            speakerDesc.text = scriptData.speakerData.spearkDesc;
            string talKScript = scriptData.script;

            // 초상화 트윈
            portraitTween.DORestartById("0");
            if (scriptData.speakerData.portraits.Length == 0)
            {
                portrait.image.color = Color.clear;
            }
            else
            {
                portrait.image.color = Color.white;
                portrait.image.sprite = scriptData.speakerData.portraits[scriptData.spriteIndex];
            }

            // Speaker가 달라지면
            if (scriptData.speakerData != prevSpeaker)
            {
                prevSpeaker = scriptData.speakerData;
                var bodyColor = scriptData.speakerData.baseColor;
                var dark = scriptData.speakerData.dark;
                var eyeColor = scriptData.speakerData.eyeColor;
                portrait.SetColor(bodyColor, dark, eyeColor);
            }

            // 스크립트 한 글자씩 출력
            for (int j = 0; j < talKScript.Length; j++)
            {
                // 대화 스킵
                if (isPressKey)
                {
                    isPressKey = false;
                    scriptText.text = talKScript;
                    if (scriptData.speakerData.talkSfx != null)
                    {
                        AudioManager.instance.PlayTalkSfx(scriptData.speakerData);
                    }
                    break;
                }
                scriptText.text += talKScript[j];
                if (scriptData.speakerData.talkSfx != null)
                {
                    AudioManager.instance.PlayTalkSfx(scriptData.speakerData);
                }
                yield return Utils.delay0_05;
            }

            isScriptEnd = true;
            yield return Utils.delay0_25;

            yield return untilPressKey;
            curTalkIndex++;
        }

        panelTween.DORestartById("Hide");
        OnTalkEnd?.Invoke();
        isTalking = false;

        yield return Utils.delay1;
        isAllowTalk = true;
    }

    public void SetWait()
    {
        isWait = true;
    }

    public void NextTalk()
    {
        isWait = false;
    }
}
