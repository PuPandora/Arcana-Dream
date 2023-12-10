using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    public CinemachineVirtualCamera placeZoomCam;

    [Title("UI")]
    public KeyInputGuide keyInputGuide;

    [Title("Talk Data")]
    public TalkData pandora0;
    public TalkData bell0;
    public TalkData kara0;

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
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBgmFade(AudioManager.Bgm.Lobby);
        }

        if (GameManager.instance.isNewGame)
        {
            TutorialManager.instance.StartTutorial(TutorialManager.TutorialType.FirstLobby);
            portal.isOpen = false;
        }

        // 스테이지에서 나온거라면
        if (GameManager.instance.isExitStage)
        {
            Vector3 portalPos = portal.transform.position + Vector3.up * 0.5f;

            player.transform.position = portalPos;
            GameManager.instance.playerCam.transform.position = portalPos;
        }
    }
}
