using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AudioManager;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Title("SFX")]
    public AudioClip[] sfxClips;
    public AudioSource[] sfxChannels;
    public byte sfxChannelCount = 16;
    public enum Sfx : short { Click, Talk = 2, expItem, Hit, Dead = 6, Coin, Step = 11 }

    public AudioSource talkSfxChannel;

    [Title("BGM")]
    public AudioClip[] BGMclips;
    public AudioSource bgmChannel;
    public enum Bgm : short { Main, Lobby, Stage, WhiteNoise }

    // Event
    public event Action OnStopBgmFadeComplete;
    public event Action OnPlayBgmFadeComplete;
    public event Action OnBgmFadeChangeComplete;

    void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
        #region 채널 초기화
        var sfx = new GameObject("SFX Channel");
        sfx.transform.SetParent(transform);
        sfxChannels = new AudioSource[sfxChannelCount];

        for (int i = 0; i < sfxChannelCount; i++)
        {
            sfxChannels[i] = sfx.AddComponent<AudioSource>();
            sfxChannels[i].playOnAwake = false;
        }

        var bgm = new GameObject("BGM Channel");
        bgm.transform.SetParent(transform);

        bgmChannel = bgm.AddComponent<AudioSource>();
        bgmChannel.playOnAwake = false;
        bgmChannel.loop = true;

        var talkSfx = new GameObject("Talk SFX Channel");
        talkSfx.transform.SetParent(transform);

        talkSfxChannel = talkSfx.AddComponent<AudioSource>();
        talkSfxChannel.playOnAwake = false;
        #endregion
    }

    void Start()
    {
        #region 볼륨 초기화
        foreach (var channel in sfxChannels)
        {
            channel.volume = DataManager.instance.optionData.sfxVolume;
        }
        talkSfxChannel.volume = DataManager.instance.optionData.sfxVolume;

        bgmChannel.volume = DataManager.instance.optionData.sfxVolume;
        #endregion
    }

    #region BGM 중단
    public void StopBgm()
    {
        bgmChannel.Stop();
    }

    public void StopBgmFade(float duration = 1f)
    {
        StartCoroutine(StopBgmFadeRoutine(duration));
    }

    public IEnumerator StopBgmFadeRoutine(float duration)
    {
        yield return bgmChannel.DOFade(0, duration).WaitForCompletion();

        OnStopBgmFadeComplete?.Invoke();
    }
    #endregion

    #region BGM 재생 
    public void PlayBgm(AudioClip clip)
    {
        bgmChannel.clip = clip;
        bgmChannel.Play();
    }

    public void PlayBgm(Bgm bgm)
    {
        bgmChannel.clip = BGMclips[(short)bgm];
        bgmChannel.Play();
    }

    public void PlayBgmFade(Bgm bgm, float duration = 1f)
    {
        bgmChannel.clip = BGMclips[(short)bgm];
        StartCoroutine(PlayBgmFadeRoutine(duration));
    }

    public void PlayBgmFade(AudioClip clip, float duration = 1f)
    {
        bgmChannel.clip = clip;

        StartCoroutine(PlayBgmFadeRoutine(duration));

        OnPlayBgmFadeComplete?.Invoke();
    }

    private IEnumerator PlayBgmFadeRoutine(float duration)
    {
        bgmChannel.volume = 0f;
        bgmChannel.Play();

        yield return bgmChannel.DOFade(DataManager.instance.optionData.bgmVoulme, duration).WaitForCompletion();

        OnPlayBgmFadeComplete?.Invoke();
    }
    #endregion

    #region BGM Fade 전환
    public void ChangeBgmFade(Bgm bgm, float duration = 1f)
    {
        bgmChannel.clip = BGMclips[(short)bgm];
        StartCoroutine(ChangeBgmFadeRoutine(duration));
    }

    public void ChangeBgmFade(AudioClip clip, float duration = 1f)
    {
        bgmChannel.clip = clip;
        StartCoroutine(ChangeBgmFadeRoutine(duration));
    }

    private IEnumerator ChangeBgmFadeRoutine(float duration)
    {
        yield return StopBgmFadeRoutine(duration);

        yield return PlayBgmFadeRoutine(duration);

        OnBgmFadeChangeComplete?.Invoke();
    }
    #endregion

    public void PlaySfx(Sfx sfx, float minPitch = 1, float maxPitch = 1)
    {
        // 사용 가능한 채널을 찾아 재생
        foreach (var channel in sfxChannels)
        {
            if (channel.isPlaying) continue;

            byte randIndex = 0;

            // Pitch 변경
            channel.pitch = Random.Range(minPitch, maxPitch);

            // 같은 사운드가 여러개의 바리에이션이 있다면
            // (많아지면 함수 분리 예정 + 랜덤이 아닌 규칙도 추가할 예정)
            switch (sfx)
            {
                case Sfx.Click:
                    randIndex = (byte)Random.Range(0, 2);
                    break;
                case Sfx.Talk:
                    break;
                case Sfx.Hit:
                    randIndex = (byte)Random.Range(0, 2);
                    channel.pitch = Random.Range(1f, 1.1f);
                    break;
                case Sfx.Coin:
                    randIndex = (byte)Random.Range(0, 4);
                    channel.pitch = Random.Range(1f, 1.1f);
                    break;
                case Sfx.Step:
                    randIndex = (byte)Random.Range(0, 3);
                    Debug.Log($"랜덤 숫지 : {randIndex}");
                    channel.pitch = Random.Range(0.7f, 0.9f);
                    break;
            }
            Debug.Log(sfx);
            Debug.Log((byte)sfx + randIndex);
            Debug.Log(randIndex);
            channel.clip = sfxClips[(short)sfx + randIndex];
            channel.Play();
            break;
        }
    }

    public void ChangeSfxVolume(float value)
    {
        foreach (var channel in sfxChannels)
        {
            channel.volume = value;
        }

        talkSfxChannel.volume = value;
    }

    public void PlayTalkSfx(SpeakerData data)
    {
        if (talkSfxChannel.isPlaying) return;

        talkSfxChannel.clip = data.talkSfx;
        talkSfxChannel.volume = DataManager.instance.optionData.sfxVolume * data.volumeIntensity;
        talkSfxChannel.pitch = Random.Range(data.pitch.x, data.pitch.y);

        talkSfxChannel.Play();
    }

    public void ChangeBgmVolume(float value)
    {
        bgmChannel.volume = value;
    }
}
