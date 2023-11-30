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

        // 테이블을 읽어 스폰 조건이 충족한다면
        if (stageData.enemyTables[stageLevel].spawnDelay < spawnTimer)
        {
            var table = stageData.enemyTables[stageLevel];
            var enemyData = stageData.enemyTables[stageLevel].enemyData;
            var index = CalculateSpawnRate(table);

            Spawn(enemyData[index]);
        }
            
        if (stageData.enemyTables[stageLevel].nextTableTime < StageManager.instance.timer)
        {
            stageLevel++;
            if (stageLevel >= stageData.enemyTables.Length)
            {
                StageManager.instance.GameClear();
            }
        }
    }

    private int CalculateSpawnRate(StageEnemyTable table)
    {
        int result = 0;
        
        float totalRate = 0f;
        foreach (var a in table.spawnRate)
        {
            totalRate += a;
        }
        float randNum = Random.Range(0f, totalRate);

        // 오름차순 정렬 (선택 정렬)
        float[] rateArr = table.spawnRate;
        for (int i = 0; i < rateArr.Length; i++)
        {
            float least = rateArr[i];
            float tmp;

            for (int j = i + 1; j < rateArr.Length; j++)
            {
                if (least < rateArr[j])
                {
                    least = rateArr[j];
                }

                tmp = rateArr[i];
                rateArr[i] = rateArr[j];
                rateArr[j] = tmp;
            }
        }

        // 확률 탐색
        for (int i = 0; i < rateArr.Length; i++)
        {
            if (randNum < rateArr[i])
            {
                result = i;
                return result;
            }

            randNum -= table.spawnRate[i];
        }

        return result;
    }

    private void Spawn(EnemyData data)
    {
        if (spawnTimer > spawnDelay)
        {
            spawnTimer = 0f;

            var item = GameManager.instance.poolManager.Get(PoolType.Enemy);
            var enemy = item.GetComponent<Enemy>();

            enemy.Initalize(data);
            enemy.target = GameManager.instance.player.transform;

            enemy.transform.position = GetRandomSpawnPoint();
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(1, spawnPoints.Length)].position;
    }

    public void Stop()
    {
        isPlaying = false;
    }
}
