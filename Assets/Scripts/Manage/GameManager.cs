using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Lobby, Stage, Pause }
public enum PlayerState { None, Shop }

public class GameManager : MonoBehaviour
{
    [Title("# State")]
    public static GameManager instance { get; private set; }
    [EnumToggleButtons]
    public GameState gameState;
    [ReadOnly]
    public PlayerState playerState;
    public bool isAllowDevMenu;

    [Title("# Managers")]
    public PoolManager poolManager;
    public DataManager dataManager;

    [Title("# Game Objects")]
    public Player player;
    public BoxCollider2D viewArea;
    public GameObject levelUpUi;
    [HideInInspector] public Inventory inventory;

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
            DontDestroyOnLoad(gameObject);
            playerStates = new PlayerStates();
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

        // Test
        OnGoldChanged += PrintGoldAmount;

    }

    private void PrintGoldAmount()
    {
        Debug.Log(gold);
    }

    public void Stop()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void EnterStage(string sceneName)
    {
        gameState = GameState.Stage;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitStage()
    {
        gameState = GameState.Lobby;
        SceneManager.LoadScene("Lobby");
    }

    public void EnterLobby()
    {
        gameState = GameState.Lobby;
        SceneManager.LoadScene("Lobby");
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
        }
        else if (scene.name.Equals("Main"))
        {
            Debug.Log("메인 메뉴 진입");
        }
        else
        {
            Debug.Log("알 수 없는 씬 진입");
        }
    }

    public void SaveGame()
    {
        dataManager.SaveGame();
    }

    public void LoadGame()
    {
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
