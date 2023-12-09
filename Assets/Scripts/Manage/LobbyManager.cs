using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("LobbyManage가 2개 이상입니다.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(AudioManager.instance.PlayBgmFade(AudioManager.Bgm.Lobby));
    }
}
