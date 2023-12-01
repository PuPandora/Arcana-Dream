using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class StageEnemyTable
{
    public EnemyData[] enemyData;
    public short[] enemyIds;
    public float spawnDelay;
    public float[] spawnRates;
    public float nextTableTime;
}

[Serializable]
public class StageData
{
    public StageEnemyTable[] enemyTables;
}

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
    private StageEnemyTable[] enemyTables;
    public StageData stageData;

    [Title("# Player Info")]
    public Player player;
    public float health;
    public float maxHealth = 100f;
    public bool isLive { get; private set; } = true;

    [Title("# Game Objects")]
    public Spawner spawner;

    public event Action OnKillCountChanged;
    public event Action OnExpChanged;
    public event Action OnLevelChanged;
    public event Action OnHealthChanged;
    public event Action OnGameOver;
    public event Action OnGameClear;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        LoadStageData();
        InitEnemyTables();
    }

    void Start()
    {
        isLive = true;
        health = maxHealth;

        stageData.enemyTables = enemyTables;
        spawner.stageData = stageData;
        spawner.ReadStageData(spawner.stageLevel);
    }

    private void InitEnemyTables()
    {
        enemyTables = new StageEnemyTable[stageData.enemyTables.Length];

        // Enemy Table 초기화
        for (int i = 0; i < enemyTables.Length; i++)
        {
            enemyTables[i] = stageData.enemyTables[i];
        }
    }

    void Update()
    {
        if (!isLive) return;

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
        health -= value;

        if (health <= 0)
        {
            health = 0;
            GameOver();
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

    [ContextMenu("Game Over")]
    private void GameOver()
    {
        isLive = false;
        spawner.isPlaying = false;
        GameManager.instance.Stop();

        OnGameOver?.Invoke();
    }

    [ContextMenu("Game Clear")]
    public void GameClear()
    {
        isLive = false;
        spawner.isPlaying = false;
        GameManager.instance.Stop();
        
        OnGameClear?.Invoke();
    }

    // 에디터 상태에서 저장
    [ContextMenu("Save Stage Table")]
    private void SaveStageData()
    {
        Debug.Log($"스테이지{stageId} 데이터 저장");

        string path = Path.Combine(Application.dataPath + $"/Data/StageTableData/Stage_{stageId}.json");

        for (int i = 0; i < this.stageData.enemyTables.Length; i++)
        {
            var enemyTable = this.stageData.enemyTables[i];
            enemyTable.enemyIds = new short[enemyTable.enemyData.Length];

            for (int j = 0; j < enemyTable.enemyIds.Length ; j++)
            {
                var enemyIds = enemyTable.enemyIds;
                var enemyDataId = enemyTable.enemyData[j].id;

                enemyIds[j] = enemyDataId;
            }
        }

        string stageData = JsonUtility.ToJson(this.stageData, true);
        File.WriteAllText(path, stageData);
    }

    [ContextMenu("Load Stage Table")]
    private void LoadStageData()
    {
        Debug.Log($"스테이지{stageId} 데이터 불러오기");

        string path = Path.Combine(Application.dataPath + $"/Data/StageTableData/Stage_{stageId}.json");

        string stageData = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(stageData, this.stageData);

        // 저장된 EnemyIds로 적 데이터를 가져와 할당
        for (int i = 0; i < this.stageData.enemyTables.Length; i++)
        {
            var enemyTable = this.stageData.enemyTables[i];

            for (int j = 0; j < enemyTable.enemyIds.Length; j++)
            {
                var enemyData = enemyTable.enemyData;
                var enemyId = enemyTable.enemyIds[j];

                enemyData[j] = Utils.GetEnemyDataWithId(enemyId);
            }
        }
    }
}
