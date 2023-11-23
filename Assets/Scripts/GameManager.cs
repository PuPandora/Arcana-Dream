using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Title("# Components")]
    public PoolManager poolManager;
    public Player player;
    public Spawner spawner;
    public BoxCollider2D viewArea;

    [Title("# Weapons")]
    public GameObject[] weapons;

    [Title("# UI")]
    public Toggle spawnToggle;

    [Title("# Stage Info")]
    public int killCount;
    public int curExp;
    public int[] nextExp = { 10, 20, 40, 60, 100, 150, 200, 300, 400, 500, 700, 1000 };
    public short level;

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

    public void GetExp(int value)
    {
        curExp += value;

        if (curExp >= nextExp[level])
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        int temp = nextExp[level] - curExp;

        level++;
        curExp = 0;
        curExp += temp;
    }
}
