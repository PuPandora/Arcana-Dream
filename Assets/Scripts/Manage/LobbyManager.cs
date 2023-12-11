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
    public NPC kara;

    [Title("GameObject")]
    public PortalZone portal;
    public CinemachineVirtualCamera placeZoomCam;
    public Transform defaultPandoraPos;

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

        if (GameManager.instance.isNeedLoad)
        {
            GameManager.instance.LoadGame();
            GameManager.instance.isNeedLoad = false;
        }

        if (TutorialManager.instance.tutorialIndex != 1)
        {
            pandora.transform.position = defaultPandoraPos.position;
        }
    }

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBgmFade(AudioManager.Bgm.Lobby);
        }

        if (TutorialManager.instance.tutorialIndex == 1)
        {
            TutorialManager.instance.StartTutorial(TutorialManager.TutorialType.FirstLobby);
            portal.isOpen = false;
        }

        if (GameManager.instance.isNeedSave)
        {
            GameManager.instance.SaveGame();
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
