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
        #region Singleton
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
        #region Make Channels
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
        foreach (var channel in sfxChannels)
        {
            if (channel.isPlaying) continue;

            Debug.Log("준비된 SFX 채널을 찾았습니다");
            byte randIndex = 0;
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
}
