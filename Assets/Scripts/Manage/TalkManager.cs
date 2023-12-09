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
    }

    public void ShowUI()
    {
        // Tweening
        panelTween.DORestartById("Show");

        //speakerPortrait.sprite = talkData.talkSession0[0].speakerData.portrait;

        StartCoroutine(Talk(talkData.talkSession0));
    }

    // To do
    // 대화 출력 중 상호작용 키를 누르면 해당 스크립트 한 번에 출력
    // 대화 소리 SFX 추가
    public IEnumerator Talk(ScriptData[] data)
    {
        isTalking = true;
        isAllowTalk = false;
        short prevSpriteIndex = -1;
        SpeakerData speaker = null;
        OnTalkStart?.Invoke();

        for (int i = 0; i < data.Length; i++)
        {
            // 초기화
            scriptText.text = string.Empty;
            isScriptEnd = false;
            isPressKey = false;
            speakerkName.text = data[i].speakerData.speakerName;
            speakerDesc.text = data[i].speakerData.spearkDesc;

            // 초상화 변경시 트윈
            // Speaker가 다르거나, 스프라이트가 달라진다면
            if (prevSpriteIndex != data[i].spriteIndex || data[i].speakerData != speaker)
            {
                portraitTween.DORestartById("0");
                portrait.image.sprite = data[i].speakerData.portraits[data[i].spriteIndex];
                speaker = data[i].speakerData;
                prevSpriteIndex = data[i].spriteIndex;
            }

            // 스크립트 한 글자씩 출력
            for (int j = 0; j < data[i].script.Length; j++)
            {
                scriptText.text += data[i].script[j];
                Debug.Log(data[i].script[j]);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Talk);
                yield return Utils.delay0_05;
            }

            isScriptEnd = true;
            isPressKey = false;
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
