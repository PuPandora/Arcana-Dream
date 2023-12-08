using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Lobby, Stage, Pause }
public enum PlayerState { None, Shop, SelectMap }

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

    [Title("# Managers")]
    public PoolManager poolManager;
    public DataManager dataManager;

    [Title("# Game Objects")]
    public Player player;
    public BoxCollider2D viewArea;
    public GameObject levelUpUi;
    [HideInInspector] public Inventory inventory;
    public CinemachineVirtualCamera vCam;

    [Title("# Weapons")]
    public PlayerWeaponController[] weapons;

    [Title("# Data")]
    [field: SerializeField]
    public ItemData[] itemDataBase { get; private set; }
    public EnemyData[] enemyDataBase;
    public PlayerStates playerStates;

    public long gold { get; private set; }

    [Title("# Key")]
    public KeyCode interactKey = KeyCode.E;

    // Event
    public event Action OnGoldChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += CheckScene;
        weapons = new PlayerWeaponController[16];

        inventory = GetComponent<Inventory>();

        // Sort Item Data Base
        Array.Sort(itemDataBase, Utils.CompareDataId);
        Array.Sort(enemyDataBase, Utils.CompareDataId);
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
        targetScene = $"Stage";
    }

    public void ExitStage()
    {
        SceneManager.LoadScene("Loading");
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

    public void LoadGame()
    {
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

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
