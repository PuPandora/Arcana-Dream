using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;
    public Player player;

    [Title("NPC")]
    public NPC pandora;
    public NPC bell;

    [Title("GameObject")]
    public PortalZone portal;

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
        AudioManager.instance.PlayBgmFade(AudioManager.Bgm.Lobby);

        // 스테이지에서 나온거라면
        if (GameManager.instance.isExitStage)
        {
            Vector3 portalPos = portal.transform.position + Vector3.up * 0.5f;

            player.transform.position = portalPos;
            GameManager.instance.playerCam.transform.position = portalPos;
        }
    }
}
