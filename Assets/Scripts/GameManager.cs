using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager poolManager;
    public Player player;
    public BoxCollider2D viewArea;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);
    }

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
