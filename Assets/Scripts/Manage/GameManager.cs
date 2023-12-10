using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Lobby, Stage, Pause }
public enum PlayerState { None, Shop, SelectMap, Talk, Tutorial }

public class GameManager : MonoBehaviour
{
    [Title("# State")]
    public static GameManager instance { get; private set; }
    [EnumToggleButtons]
    public GameState gameState;
    [ReadOnly]
    public PlayerState playerState;
    public bool isAllowDevMenu;
    public string targetScene;
    public bool isNeedLoad = true;
    public bool isNewGame = false;
    public bool isExitStage;

    [Title("# Managers")]
    public PoolManager poolManager;
    public DataManager dataManager;

    [Title("# Game Objects")]
    public Player player;
    public BoxCollider2D viewArea;
    public GameObject levelUpUi;
    [HideInInspector] public Inventory inventory;
    public CinemachineVirtualCamera playerCam;

    [Title("# Weapons")]
    public PlayerWeaponController[] weapons;

    [Title("# Data")]
    [field: SerializeField]
    public ItemData[] itemDataBase { get; private set; }
    public EnemyData[] enemyDataBase;
    public PlayerStates playerStates;

    public long gold { get; private set; }

    [Title("# Input")]
    public bool isInputInteract;

    // Event
    public event Action OnGoldChanged;

    void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        #endregion

        SceneManager.sceneLoaded += CheckScene;
        inventory = GetComponent<Inventory>();
        // Sort Item Data Base
        Array.Sort(itemDataBase, Utils.CompareDataId);
        Array.Sort(enemyDataBase, Utils.CompareDataId);
        weapons = new PlayerWeaponController[16];
    }

    public void Stop()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void EnterStage()
    {
        SceneManager.LoadScene("Loading");
        AudioManager.instance.StopBgm();
        targetScene = $"Stage";
    }

    public void ExitStage()
    {
        isExitStage = true;

        SceneManager.LoadScene("Loading");
        AudioManager.instance.StopBgm();
        targetScene = "Lobby";
    }

    public void EnterLobby()
    {
        SceneManager.LoadScene("Loading");
        targetScene = "Lobby";
    }

    private void CheckScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Stage"))
        {
            gameState = GameState.Stage;
            Debug.Log("스테이지 진입");
            DOTween.SetTweensCapacity(tweenersCapacity: 1500, sequencesCapacity: 200);
        }
        else if (scene.name.Equals("Lobby"))
        {
            gameState = GameState.Lobby;
            Debug.Log("로비 진입");
            if (isNeedLoad)
            {
                LoadGame();
            }
        }
        else if (scene.name.Equals("Main"))
        {
            Debug.Log("메인 메뉴 진입");
        }
        else if (scene.name.Equals("Loading"))
        {
            Debug.Log("로딩 씬 진입");
        }
        else
        {
            Debug.Log("알 수 없는 씬 진입");
        }
    }

    public void SaveGame()
    {
        if (dataManager == null)
        {
            Debug.LogError("데이터 매니저가 없습니다. 저장 실패");
            return;
        }

        dataManager.SaveGame();
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        Debug.Log(dataManager == null);
        if (dataManager == null)
        {
            Debug.LogError("데이터 매니저가 없습니다. 로드 실패");
            return;
        }

        dataManager.LoadGame();
    }

    public void ClearInventory()
    {
        inventory.ClearInventory();
        dataManager.SaveGame();
    }

    public void SetGold(long value)
    {
        gold = value;
        OnGoldChanged?.Invoke();
    }

    public void OnNewGameButton()
    {
        isNewGame = true;
    }

    public void OnContinueGameButton()
    {
        isNewGame = false;
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnSubmit(InputValue value)
    {
        switch (playerState)
        {
            case PlayerState.None:
                StartCoroutine(InteractRoutine());
                break;
            case PlayerState.Shop:
                break;
            case PlayerState.Talk:
                TalkManager.instance.isPressKey = true;
                break;
            case PlayerState.Tutorial:
                TutorialManager.instance.isPressKey = true;
                break;
        }
    }

    private void OnCancel(InputValue value)
    {
        Debug.Log("ESC 키, 취소 키");

        UIManager.instance.CloseCurrentUI();
    }

    private IEnumerator InteractRoutine()
    {
        isInputInteract = true;

        yield return null;

        isInputInteract = false;
    }

    public void ChangePlayerState(PlayerState state)
    {
        playerState = state;
    }
}
