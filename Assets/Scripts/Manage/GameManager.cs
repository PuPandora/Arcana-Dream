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

    [Title("# Game Objects")]
    public PoolManager poolManager;
    public Player player;
    public Spawner spawner;
    public BoxCollider2D viewArea;
    public GameObject levelUpUi;

    [Title("# Player Info")]
    public float health;
    public float maxHealth = 100f;

    [Title("# Weapons")]
    public PlayerWeaponController[] weapons;

    [Title("# UI")]
    public Toggle spawnToggle;
    public Button[] debugWeaponBtns = new Button[3];

    [Title("# Stage Info")]
    public int killCount;
    public int curExp;
    public int[] nextExp = { 10, 20, 40, 60, 100, 150, 200, 300, 400, 500, 700, 1000 };
    public short level;
    public float timer { get; private set; }

    // Events
    public Action OnKillCountChanged;
    public Action OnExpChanged;
    public Action OnLevelChanged;
    public Action OnHealthChanged;

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

        health = maxHealth;
    }

    void Update()
    {
        if (gameState == GameState.Stage && spawner.isPlaying)
        {
            timer += Time.deltaTime;
        }
    }

    public void GetExp(int value)
    {
        curExp += value;

        if (curExp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            LevelUp();
        }

        OnExpChanged?.Invoke();
    }

    public void AddKillCount()
    {
        killCount++;
        OnKillCountChanged?.Invoke();
    }

    private void LevelUp()
    {
        int temp = nextExp[Mathf.Min(level, nextExp.Length - 1)] - curExp;

        level++;
        curExp = 0;
        curExp += temp;

        OnLevelChanged?.Invoke();

        Stop();
    }

    public void GetDamaged(float value)
    {
        health -= value;

        if (health <= 0)
        {
            health = 0;
        }

        OnHealthChanged?.Invoke();
    }

    public void Heal(float value)
    {
        health += value;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        OnHealthChanged?.Invoke();
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
        timer = 0f;
        level = 0;
        curExp = 0;
        killCount = 0;
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
}
