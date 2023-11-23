using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoints;

    private float timer;
    public float spawnDelay = 0.1f;
    public bool canSpawn = true;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (canSpawn)
        {
            timer += Time.deltaTime;
            Spawn();
        }
    }

    public void Spawn()
    {
        if (timer > spawnDelay)
        {
            timer = 0f;

            var item = GameManager.instance.poolManager.Get(PoolType.Enemy);
            var enemy = item.GetComponent<Enemy>();

            enemy.target = GameManager.instance.player.transform;
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        }
    }
}
