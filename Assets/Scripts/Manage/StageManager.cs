using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Title("# Stage Info")]
    public byte stageId;
    public int killCount;
    public int curExp;
    public int[] nextExp = { 10, 20, 40, 60, 100, 150, 200, 300, 400, 500, 700, 1000 };
    public short level;
    public float timer { get; private set; }
    public bool isPlaying;
    public bool isTutorial;
    [SerializeField] private Light2D globalLight;
    [SerializeField] Transform grid;
    private StageTable[] enemyTables;
    public StageData stageData;

#if UNITY_EDITOR
    [Tooltip("불러올 StageData 파일 이름")]
    [SerializeField] private TextAsset stageDataFile;
#endif

    [Title("# Player Info")]
    public Player player;
    public float health;
    public bool isLive { get; private set; } = true;

    [Title("# Game Objects")]
    public Spawner spawner;
    public ItemCollector itemCollector;
    public HUD hud;
    public GetItemUI[] getItemUis;
    public Transition transition;

    public event Action OnKillCountChanged;
    public event Action OnExpChanged;
    public event Action OnLevelChanged;
    public event Action OnHealthChanged;
    public event Action OnGameOver;
    public event Action OnGameClear;

    void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
        InitEnemyTables();
    }

    void Start()
    {
        // 기본 초기화
        SettingGrid();
        globalLight.intensity = stageData.stageGlobalLightIntensity;
        isLive = true;
        health = GameManager.instance.playerStates.health;
        hud.UpdateHealthSlider();

        // 스폰 정보 초기화
        spawner.Initialize();
        stageData.stageTable = enemyTables;

        // BGM
        if (stageData.bgm != null)
        {
            AudioManager.instance.PlayBgmFade(stageData.bgm);
        }
        else
        {
            Debug.LogError($"Stage Data에 등록된 BGM이 없습니다.\n데이터 이름 : {stageData.name}", gameObject);
        }

        // 튜토리얼
        isTutorial = TutorialManager.instance.tutorialIndex == 0;
        if (isTutorial)
        {
            isPlaying = false;
            stageData =  TutorialManager.instance.tutorialStageData;
            StartCoroutine(TutorialManager.instance.FirstTutorialRoutine());
        }
        else
        {
            transition.Open();
            hud.canvasGroup.alpha = 1;
            hud.cameraCanvasGroup.alpha = 1;
            isPlaying = true;
        }
    }

    private void InitEnemyTables()
    {
        enemyTables = new StageTable[stageData.stageTable.Length];

        // Enemy Table 초기화
        for (int i = 0; i < enemyTables.Length; i++)
        {
            enemyTables[i] = stageData.stageTable[i];
        }
    }

    void Update()
    {
        if (!isLive || !isPlaying) return;

        timer += Time.deltaTime;
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

        GameManager.instance.Stop();
    }

    public void GetDamaged(float value)
    {
        if (!isLive) return;

        var damage = value - GameManager.instance.playerStates.defense;
        health -= damage < Time.deltaTime ? Time.deltaTime : damage;

        if (health <= 0)
        {
            if (isTutorial)
            {
                health = GameManager.instance.playerStates.health;
                return;
            }
            health = 0;
            GameOver();
        }

        OnHealthChanged?.Invoke();
    }

    public void Heal(float value)
    {
        health += value;

        if (health >= GameManager.instance.playerStates.health)
        {
            health = GameManager.instance.playerStates.health;
        }

        OnHealthChanged?.Invoke();
    }

    [ContextMenu("Game Over")]
    private void GameOver()
    {
        isLive = false;
        isPlaying = false;
        //GameManager.instance.Stop();

        OnGameOver?.Invoke();
    }

    [ContextMenu("Game Clear")]
    public void GameClear()
    {
        isLive = false;
        isPlaying = false;

        var enemyPool = GameManager.instance.poolManager.GetPool(PoolType.Enemy);
        foreach (var enemy in enemyPool)
        {
            enemy.GetComponent<Enemy>().Hit(999999999.0f);
        }

        foreach (var item in GameManager.instance.poolManager.GetPool(PoolType.DropItem))
        {
            StartCoroutine(item.GetComponent<Item>().MoveToPlayerRoutine());
        }

        OnGameClear?.Invoke();
    }

    [ContextMenu("Save Stage Data to Json")]
    private void SaveStageData()
    {
        Utils.SaveStageData(stageData);
    }

    [ContextMenu("Load Stage Data from Json")]
    private void LoadStageData()
    {
        if (stageDataFile == null)
        {
            Debug.LogError("Stage Data File이 비어있습니다.");
            return;
        }

        Utils.LoadStageData(stageDataFile.ToString(), stageData);
    }

    private void SettingGrid()
    {
        Instantiate(stageData.stageFloor, new Vector3(-15, 15, 0), Quaternion.identity, grid);
        Instantiate(stageData.stageFloor, new Vector3(15, 15, 0), Quaternion.identity, grid);
        Instantiate(stageData.stageFloor, new Vector3(15, -15, 0), Quaternion.identity, grid);
        Instantiate(stageData.stageFloor, new Vector3(-15, -15, 0), Quaternion.identity, grid);
    }
}
