using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager poolManager;
    public Player player;
    public Spawner spawner;
    public BoxCollider2D viewArea;

    public GameObject[] weapons;

    // UI
    public Toggle spawnToggle;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);
    }

    public void ActiveWeapon(int index)
    {
        weapons[index].SetActive(true);
    }

    public void DeactiveAllWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }
    }

    void Update()
    {
        // 스폰 토글 디버그
        spawner.canSpawn = spawnToggle.isOn;
    }
}
