using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Title("# Stage Info")]
    public int killCount;
    public int curExp;
    public int[] nextExp = { 10, 20, 40, 60, 100, 150, 200, 300, 400, 500, 700, 1000 };
    public short level;
    public float timer { get; private set; }

    [Title("# Player Info")]
    public Player player;
    public float health;
    public float maxHealth = 100f;
    public bool isLive { get; private set; } = true;

    [Title("# Game Objects")]
    public Spawner spawner;

    public Action OnKillCountChanged;
    public Action OnExpChanged;
    public Action OnLevelChanged;
    public Action OnHealthChanged;
    public Action OnGameOver;
    public Action OnGameClear;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        isLive = true;
        health = maxHealth;
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
    private void GameClear()
    {
        OnGameClear?.Invoke();
    }
}
