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
    public Image speakerPortrait;
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

    [Title("Tween")]
    [SerializeField] DOTweenAnimation panelTween;
    [SerializeField] DOTweenAnimation portraitTween;

    // Event
    public event Action OnTalkStart;
    public event Action OnTalkEnd;

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
    }

    public void Update()
    {
        if (isTalking && !isPressKey && isScriptEnd && Input.GetKeyDown(GameManager.instance.interactKey))
        {
            isPressKey = true;
        }
    }

    public void ShowUI()
    {
        // Tweening
        panelTween.DORestartById("Show");

        // 임시 대화 상대 정보 불러오기 코드
        speakerkName.text = talkData.talkSession0[0].speakerData.speakerName;
        speakerDesc.text = talkData.talkSession0[0].speakerData.spearkDesc;
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
        OnTalkStart?.Invoke();

        for (int i = 0; i < data.Length; i++)
        {
            scriptText.text = string.Empty;
            isScriptEnd = false;
            isPressKey = false;

            for (int j = 0; j < data[i].script.Length; j++)
            {
                scriptText.text += data[i].script[j];
                Debug.Log(data[i].script[j]);
                yield return Utils.delay0_05;
            }
            isScriptEnd = true;
            yield return new WaitUntil(() => isPressKey);

            // 임시 초상화 트윈 코드
            if (i >= data.Length - 1) continue;
            portraitTween.DORestartById("0");
        }

        panelTween.DORestartById("Hide");
        OnTalkEnd?.Invoke();
        isTalking = false;

        yield return Utils.delay1;
        isAllowTalk = true;
    }
}