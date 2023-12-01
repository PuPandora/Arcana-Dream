using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoints;
    public StageData stageData;

    [ReadOnly]
    [SerializeField]
    private float spawnTimer;

    public float spawnDelay = 0.1f;
    public bool isPlaying = true;

    public byte stageLevel;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        stageLevel = 0;
    }

    void Start()
    {
        StageManager.instance.OnGameClear += Stop;
    }

    void Update()
    {
        if (!isPlaying) return;

        spawnTimer += Time.deltaTime;

        // 스폰 테이블에 따라 적 스폰
        if (spawnDelay < spawnTimer)
        {
            var table = stageData.enemyTables[stageLevel];
            var enemyData = CalculateSpawnRate(table);

            Spawn(enemyData);
        }
            
        // 다음 테이블로 넘어갈 시간이 되면
        if (stageData.enemyTables[stageLevel].nextTableTime < StageManager.instance.timer)
        {
            stageLevel++;
            // 테이블이 남아있다면 읽기
            if (stageLevel < stageData.enemyTables.Length)
            {
                ReadStageData(stageLevel);
                
            }
            // 끝이라면 게임 클리어
            else
            {
                StageManager.instance.hud.UpdateTimerText();
                StageManager.instance.GameClear();
            }
        }
    }

    private EnemyData CalculateSpawnRate(StageEnemyTable table)
    {
        if (table.enemyData.Length != table.spawnRates.Length)
        {
            Debug.LogWarning($"스폰 테이블의 적 데이터 개수와 스폰 확률 개수가 같지 않습니다.");
        }

        EnemyData result = null;
        float totalRate = 0f;
        
        // 확률 더하기
        foreach (var rate in table.spawnRates)
        {
            totalRate += rate;
        }

        // 확률 데이터 구조체 초기화
        RateData[] rateData = new RateData[table.spawnRates.Length];
        for (int i = 0; i < rateData.Length; i++)
        {
            rateData[i].enemyData = table.enemyData[i];
            rateData[i].spawnRate = table.spawnRates[i];
        }

        // 오름차순 정렬 (선택 정렬)
        for (int i = 0; i < rateData.Length; i++)
        {
            float least = rateData[i].spawnRate;
            float tmp;

            for (int j = i + 1; j < rateData.Length; j++)
            {
                if (least < rateData[j].spawnRate)
                {
                    least = rateData[j].spawnRate;
                }

                tmp = rateData[i].spawnRate;
                rateData[i].spawnRate = rateData[j].spawnRate;
                rateData[j].spawnRate = tmp;
            }
        }

        // 확률 계산
        float randNum = Random.Range(0f, totalRate);
        for (int i = 0; i < rateData.Length; i++)
        {
            // 스폰 확률이 통과 됐다면
            if (randNum < rateData[i].spawnRate)
            {
                result = rateData[i].enemyData;
                break;
            }

            randNum -= table.spawnRates[i];
        }

        return result;
    }

    // 스폰 확률 계산용 구조체
    struct RateData
    {
        public RateData(byte length)
        {
            enemyData = null;
            spawnRate = 0f;
        }

        public EnemyData enemyData;
        public float spawnRate;
    }

    private void Spawn(EnemyData data)
    {
        spawnTimer = 0f;

        var item = GameManager.instance.poolManager.Get(PoolType.Enemy);
        var enemy = item.GetComponent<Enemy>();

        enemy.Initalize(data);
        enemy.target = GameManager.instance.player.transform;

        enemy.transform.position = GetRandomSpawnPoint();
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(1, spawnPoints.Length)].position;
    }

    public void Stop()
    {
        isPlaying = false;
    }

    public void ReadStageData(byte index)
    {
        spawnDelay = stageData.enemyTables[index].spawnDelay;
    }
}
