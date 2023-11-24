using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoints;

    [ReadOnly]
    [SerializeField]
    private float spawnTimer;

    public float spawnDelay = 0.1f;
    public bool isPlaying = true;

    public byte stageLevel;
    public EnemyData[] enemyData;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (isPlaying)
        {
            spawnTimer += Time.deltaTime;
            // 임시 스테이지 레벨 코드
            stageLevel = (byte)(GameManager.instance.timer / 10f);
            if (stageLevel > enemyData.Length - 1)
            {
                stageLevel = (byte)(enemyData.Length - 1);
                isPlaying = false;
                return;
            }
            Spawn();
        }
    }

    public void Spawn()
    {
        if (spawnTimer > spawnDelay)
        {
            spawnTimer = 0f;

            var item = GameManager.instance.poolManager.Get(PoolType.Enemy);
            var enemy = item.GetComponent<Enemy>();

            enemy.Initalize(enemyData[stageLevel]);
            enemy.target = GameManager.instance.player.transform;
            enemy.transform.position = GetRandomSpawnPoint();
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(1, spawnPoints.Length)].position;
    }
}
