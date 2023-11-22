using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PoolManager poolManager;
    public Player player;

    void Update()
    {
        // Test Code. Spawn Enemy
        if (Input.GetButtonDown("Jump"))
        {
            var item = poolManager.Get(PoolType.Enemy);
            var enemy = item.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.transform.position = Vector3.zero;
            enemy.transform.rotation = Quaternion.identity;
        }
    }
}
