using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static StageTable;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoints;

    [ReadOnly]
    [SerializeField]
    private float spawnTimer;

    public byte stageLevel;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        stageLevel = 0;
    }

    public void Initialize()
    {
        StageManager.instance.OnGameClear += Stop;
    }

    void Update()
    {
        if (!StageManager.instance.isPlaying || !StageManager.instance.isLive) return;

        spawnTimer += Time.deltaTime;
        float delay = StageManager.instance.stageData.stageTable[stageLevel].spawnDelay;

        // 스폰 테이블에 따라 적 스폰
        if (delay < spawnTimer)
        {
            var table = StageManager.instance.stageData.stageTable[stageLevel].spawnTable;
            var enemyData = CalculateSpawnRate(table);

            Spawn(enemyData);
        }
            
        float nextTableTime = StageManager.instance.stageData.stageTable[stageLevel].nextTableTime;

        // 다음 테이블로 넘어갈 시간이 되면
        if (nextTableTime < StageManager.instance.timer)
        {
            stageLevel++;
            Debug.Log($"Stage Data : {StageManager.instance.stageData.name}");
            Debug.Log($"Stage Level Length : {StageManager.instance.stageData.stageTable.Length}");
            Debug.Log($"Stage Next Table Time : {nextTableTime}");
            // 읽을 다음 테이블이 없다면
            if (stageLevel >= StageManager.instance.stageData.stageTable.Length)
            {
                StageManager.instance.hud.UpdateTimerText();
                StageManager.instance.GameClear();
            }
        }
    }

    private EnemyData CalculateSpawnRate(SpawnTable[] tables)
    {
        EnemyData result = null;
        float totalRate = 0f;

        // 확률 더하기
        foreach (var table in tables)
        {
            totalRate += table.spawnWeight;
        }

        // 확률 데이터 구조체 초기화
        RateData[] rateData = new RateData[tables.Length];
        for (int i = 0; i < rateData.Length; i++)
        {
            rateData[i].enemyData = tables[i].enemyData;
            rateData[i].spawnRate = tables[i].spawnWeight;
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

            randNum -= tables[i].spawnWeight;
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
        StageManager.instance.isPlaying = false;
    }
}
