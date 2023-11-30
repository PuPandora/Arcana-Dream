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
    public Inventory inventory;
    public DeveloperMenu devMenu;
    public GameObject devMenuGroup;
    public InventoryUI inventoryUI;

    [Title("# Weapons")]
    public PlayerWeaponController[] weapons;

    [Title("# Data")]
    [field: SerializeField]
    public ItemData[] itemDataBase { get; private set; }
    public EnemyData[] enemyDataBase;
    public int gold
    {
        get
        {
            return m_gold;
        }
        set
        {
            m_gold = value;
            OnGoldChanged?.Invoke();
        }
    }
    [field: SerializeField]
    private int m_gold = 0;

    [Title("# Key")]
    [SerializeField] KeyCode debugMenuKey = KeyCode.Escape;
    [SerializeField] KeyCode inventoryKey = KeyCode.I;
    public KeyCode interactKey = KeyCode.E;

    // Event
    public event Action OnGoldChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += CheckScene;
        weapons = new PlayerWeaponController[16];

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

    void Update()
    {
        if (Input.GetKeyDown(debugMenuKey) && isAllowDevMenu)
        {
            devMenuGroup.SetActive(!devMenuGroup.activeSelf);

            if (devMenuGroup.activeSelf)
            {
                devMenu.ShowUI();
            }
            else
            {
                devMenu.HideUI();
            }
        }

        if (Input.GetKeyDown(inventoryKey))
        {
            if (!inventoryUI.isUIOpen)
            {
                inventoryUI.ShowUI();
            }
            else
            {
                inventoryUI.HideUI();
            }
        }
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

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
