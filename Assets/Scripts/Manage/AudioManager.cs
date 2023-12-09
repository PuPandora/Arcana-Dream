using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Title("SFX")]
    public AudioClip[] sfxClips;
    public AudioSource[] sfxChannels;
    public byte sfxChannelCount = 16;
    public enum Sfx : short { Click, Talk = 2 }

    [Title("BGM")]
    public AudioClip[] BGMclips;
    public AudioSource bgmChannel;
    public enum Bgm : short { Main, Lobby, Stage, WhiteNoise }

    // Event
    public event Action OnStopBgmFadeComplete;
    public event Action OnPlayBgmFadeComplete;

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
        #endregion
    }

    void Start()
    {
        #region 볼륨 초기화
        foreach (var channel in sfxChannels)
        {
            channel.volume = DataManager.instance.optionData.sfxVolume;
        }
        bgmChannel.volume = DataManager.instance.optionData.sfxVolume;
        #endregion
    }

    public void PlayBgm(int index)
    {
        PlayBgm((Bgm)index);
    }

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

    public void StopBgm()
    {
        bgmChannel.Stop();
    }

    public IEnumerator StopBgmFade()
    {
        yield return bgmChannel.DOFade(0, 2f).WaitForCompletion();

        OnStopBgmFadeComplete?.Invoke();
    }

    public IEnumerator PlayBgmFade(Bgm bgm)
    {
        bgmChannel.clip = BGMclips[(short)bgm];
        bgmChannel.volume = 0f;
        bgmChannel.Play();
        yield return bgmChannel.DOFade(DataManager.instance.optionData.bgmVoulme, 2f).WaitForCompletion();

        OnPlayBgmFadeComplete?.Invoke();
    }

    public IEnumerator PlayBgmFade(AudioClip clip)
    {
        bgmChannel.clip = clip;
        bgmChannel.volume = 0f;
        bgmChannel.Play();
        yield return bgmChannel.DOFade(DataManager.instance.optionData.bgmVoulme, 2f).WaitForCompletion();

        OnPlayBgmFadeComplete?.Invoke();
    }

    public IEnumerator ChangeBgmFade(Bgm bgm)
    {
        yield return StopBgmFade();

        yield return PlayBgmFade(bgm);

        Debug.Log("브금 체인지 완료");
    }

    public IEnumerator ChangeBgmFade(AudioClip clip)
    {
        yield return StopBgmFade();

        yield return PlayBgmFade(clip);

        Debug.Log("브금 체인지 완료");
    }

    public void PlaySfx(Sfx sfx, float minPitch = 1, float maxPitch = 1)
    {
        // 사용 가능한 채널을 찾아 재생
        foreach (var channel in sfxChannels)
        {
            if (channel.isPlaying) continue;

            byte randIndex = 0;

            // Pitch 변경
            if (Mathf.Approximately(minPitch, 1) && Mathf.Approximately(maxPitch, 1))
            {
                channel.pitch = 1;
            }
            else
            {
                channel.pitch = Random.Range(minPitch, maxPitch);
            }

            // 같은 사운드가 여러개의 바리에이션이 있다면
            // (많아지면 함수 분리 예정 + 랜덤이 아닌 규칙도 추가할 예정)
            switch (sfx)
            {
                case Sfx.Click:
                    randIndex = (byte)Random.Range(0, 2);
                    break;
                case Sfx.Talk:
                    break;
            }
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
    }

    public void ChangeBgmVolume(float value)
    {
        bgmChannel.volume = value;
    }
}
