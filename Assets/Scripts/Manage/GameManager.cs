using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Lobby, Stage, Pause }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [EnumToggleButtons]
    public GameState gameState;
    public bool isAllowDevMenu;

    [Title("# Managers")]
    public PoolManager poolManager;
    public StageManager stageManager;
    public DataManager dataManager;

    [Title("# Game Objects")]
    public Player player;
    public BoxCollider2D viewArea;
    public GameObject levelUpUi;
    public Inventory inventory;
    public DeveloperMenu devMenu;
    public GameObject devMenuGroup;

    [Title("# Weapons")]
    public PlayerWeaponController[] weapons;

    [Title("# UI")]
    public Toggle spawnToggle;
    public Button[] debugWeaponBtns = new Button[3];

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isAllowDevMenu)
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
        SceneManager.LoadScene(sceneName);
    }

    public void ExitStage()
    {
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
}
