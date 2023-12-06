using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Title("SFX")]
    public AudioClip[] sfxClips;
    public AudioSource[] sfxChannels;
    public byte sfxChannelCount = 16;
    public enum Sfx : short { Click, }

    [Title("BGM")]
    public AudioClip[] BGMclips;
    public AudioSource[] bgmChannels;
    public byte bgmChannelCount = 2;
    public enum Bgm : short { Main, Lobby, Stage }

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
        bgmChannels = new AudioSource[bgmChannelCount];

        for (int i = 0; i < bgmChannelCount; i++)
        {
            bgmChannels[i] = bgm.AddComponent<AudioSource>();
            bgmChannels[i].playOnAwake = false;
            bgmChannels[i].loop = true;
        }
        #endregion
    }

    void Start()
    {
        #region 볼륨 초기화
        foreach (var channel in sfxChannels)
        {
            channel.volume = DataManager.instance.optionData.sfxVolume;
        }
        foreach (var channel in bgmChannels)
        {
            channel.volume = DataManager.instance.optionData.bgmVoulme;
        }
        #endregion
    }

    public void PlayBgm(Bgm bgm, bool isPlay)
    {
        if (isPlay)
        {
            foreach (var channel in bgmChannels)
            {
                if (channel.isPlaying) continue;

                Debug.Log("사용 가능한 BGM 채널 찾음");
                channel.clip = BGMclips[(short)bgm];
                channel.Play();
                break;
            }
        }
        else
        {
            foreach (var channel in bgmChannels)
            {
                if (!channel.isPlaying) continue;

                if (channel.clip != BGMclips[(int)bgm]) continue;

                channel.Stop();
            }
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        // 사용 가능한 채널을 찾아 재생
        foreach (var channel in sfxChannels)
        {
            if (channel.isPlaying) continue;

            byte randIndex = 0;
            // 같은 사운드가 여러개의 바리에이션이 있다면
            // (많아지면 함수 분리 예정 + 랜덤이 아닌 규칙도 추가할 예정)
            if (sfx == Sfx.Click)
            {
                randIndex = (byte)Random.Range(0, 2);
                channel.pitch = Random.Range(1.3f, 1.6f);
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
        foreach (var channel in bgmChannels)
        {
            channel.volume = value;
        }
    }
}
