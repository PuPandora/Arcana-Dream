using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core.Easing;

[Serializable]
public class InventoryData
{
    // Inventory
    [HideInInspector] public byte count = Utils.INVENTORY_SLOT_COUNT;
    public short[] itemIds = new short[Utils.INVENTORY_SLOT_COUNT];
    public byte[] stacks = new byte[Utils.INVENTORY_SLOT_COUNT];

    // 생성자 : 배열 초기화
    public InventoryData()
    {
        for (int i = 0; i < itemIds.Length; i++)
        {
            itemIds[i] = 0;
        }

        for (int i = 0; i < stacks.Length; i++)
        {
            stacks[i] = 0;
        }
    }

    public void PrintData()
    {
        for (int i = 0; i < Utils.INVENTORY_SLOT_COUNT; i++)
        {
            Debug.Log($"Item ID : {itemIds[i]}\nStack : {stacks[i]}");
        }
    }
}

[Serializable]
public class SaveData
{
    public PlayerStates playerStateData;
    public InventoryData inventoryData;

    // Game Data
    public long gold;
    public Vector3 position;
    public bool playerSpriteFlip;
}

public class DataManager : MonoBehaviour
{
    // 항상, 하나만 존재하며
    // GameManager 외 다른 클래스에서 접근을 막기 위한 싱글톤
    private static DataManager instance;

    public byte saveSlotIndex { get; private set; } = 0;
    public string path { get; private set; } = Application.dataPath + "/Data/SaveData/";

    GameManager gameManager;
    [SerializeField] private SaveData saveData;

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
        path = Path.Combine(path, $"SaveData_{saveSlotIndex}.json");

        gameManager = GameManager.instance;
        GameManager.instance.dataManager = this;
    }

    // 추후 비동기 방식으로 구현 예정
    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        saveData.inventoryData = gameManager.inventory.GetInventoryData();
        saveData.playerStateData = gameManager.playerStates;

        saveData.gold = gameManager.gold;
        saveData.position = gameManager.player.transform.position;
        saveData.playerSpriteFlip = gameManager.player.spriter.flipX;

        saveData.playerStateData.damageLevel = gameManager.playerStates.damageState.level;
        saveData.playerStateData.healthLevel = gameManager.playerStates.healthState.level;
        saveData.playerStateData.defenseLevel = gameManager.playerStates.defenseState.level;

    string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);

        Debug.Log($"게임 데이터 저장 완료\n경로 : {path}");
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.Log("데이터가 없습니다.");
            return;
        }

        string loadJson = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(loadJson, saveData);

        gameManager.inventory.ApplyData(saveData.inventoryData);
        gameManager.SetGold(saveData.gold);
        gameManager.player.transform.position = saveData.position;
        gameManager.vCam.transform.position = saveData.position;
        gameManager.player.spriter.flipX = saveData.playerSpriteFlip;

        LoadStatesData();

        Debug.Log("게임 데이터 불러오기 완료");
    }

    private void LoadStatesData()
    {
        // 레벨 적용
        gameManager.playerStates.damageState.level = saveData.playerStateData.damageLevel;
        gameManager.playerStates.healthState.level = saveData.playerStateData.healthLevel;
        gameManager.playerStates.defenseState.level = saveData.playerStateData.defenseLevel;


        StateUpgradeData[] statesData = new StateUpgradeData[] {
            gameManager.playerStates.damageState,
            gameManager.playerStates.healthState,
            gameManager.playerStates.defenseState 
        };

        // 레벨에 맞춰 스탯 적용
        foreach (var data in statesData)
        {
            data.ApplyState(data.level);
        }
    }
}
