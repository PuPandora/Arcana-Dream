using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using static TutorialManager;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    [Title("Player")]
    public Player player;
    public CinemachineVirtualCamera playerCam;

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
    }

    void Start()
    {
        if (GameManager.instance.isNeedLoad)
        {
            GameManager.instance.LoadGame();
            GameManager.instance.playerStates.Init();
            GameManager.instance.isNeedLoad = false;
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBgmFade(AudioManager.Bgm.Lobby);
        }

        if (GameManager.instance.isNewGame)
        {
            Vector3 portalPos = portal.transform.position + Vector3.up * 0.5f;
            player.transform.position = portalPos;
            portal.isOpen = false;
            // 임시
            TutorialManager.instance.StartTutorial(TutorialType.FirstLobby);
        }
        else
        {
            player.transform.position = DataManager.instance.saveData.position;
            playerCam.transform.position = DataManager.instance.saveData.position;
            pandora.transform.position = defaultPandoraPos.position;
        }

        if (GameManager.instance.isNeedSave)
        {
            GameManager.instance.SaveGame();
            GameManager.instance.isNeedSave = false;
        }

        // 스테이지에서 나온거라면
        if (GameManager.instance.isExitStage)
        {
            Vector3 portalPos = portal.transform.position + Vector3.up * 0.5f;

            player.transform.position = portalPos;
            GameManager.instance.playerCam.transform.position = portalPos;

            GameManager.instance.isExitStage = false;
        }
    }
}
