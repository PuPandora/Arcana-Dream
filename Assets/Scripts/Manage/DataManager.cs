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
    public PlayerStates playerStateData = new PlayerStates();
    public InventoryData inventoryData = new InventoryData();

    // Game Data
    public long gold;
    public Vector3 position;
    public bool playerSpriteFlip;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public byte saveSlotIndex = 0;
    public string path { get; private set; } = Application.dataPath + "/Data/SaveData/";
    public string saveFileName = "SaveData.json";
    public string optionFileName = "OptionData.json";

    public SaveData saveData = new SaveData();
    public OptionData optionData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        GameManager.instance.dataManager = this;
    }

    // 추후 비동기 방식으로 구현 예정
    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        saveData.inventoryData = GameManager.instance.inventory.GetInventoryData();
        saveData.playerStateData = GameManager.instance.playerStates;

        saveData.gold = GameManager.instance.gold;
        saveData.position = GameManager.instance.player.transform.position;
        saveData.playerSpriteFlip = GameManager.instance.player.spriter.flipX;

        saveData.playerStateData.damageLevel = GameManager.instance.playerStates.damageState.level;
        saveData.playerStateData.healthLevel = GameManager.instance.playerStates.healthState.level;
        saveData.playerStateData.defenseLevel = GameManager.instance.playerStates.defenseState.level;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path + saveFileName, json);

        Debug.Log($"게임 데이터 저장 완료\n경로 : {path}");
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        if (!File.Exists(path + saveFileName))
        {
            Debug.Log("데이터가 없습니다.");
            return;
        }

        string loadJson = File.ReadAllText(path + saveFileName);
        JsonUtility.FromJsonOverwrite(loadJson, saveData);

        GameManager.instance.inventory.ApplyData(saveData.inventoryData);
        GameManager.instance.SetGold(saveData.gold);
        GameManager.instance.player.transform.position = saveData.position;
        GameManager.instance.vCam.transform.position = saveData.position;
        GameManager.instance.player.spriter.flipX = saveData.playerSpriteFlip;

        LoadStatesData();

        Debug.Log("게임 데이터 불러오기 완료");
    }

    private void LoadStatesData()
    {
        // 레벨 적용
        GameManager.instance.playerStates.damageState.level = saveData.playerStateData.damageLevel;
        GameManager.instance.playerStates.healthState.level = saveData.playerStateData.healthLevel;
        GameManager.instance.playerStates.defenseState.level = saveData.playerStateData.defenseLevel;


        StateUpgradeData[] statesData = new StateUpgradeData[] {
            GameManager.instance.playerStates.damageState,
            GameManager.instance.playerStates.healthState,
            GameManager.instance.playerStates.defenseState 
        };

        // 레벨에 맞춰 스탯 적용
        foreach (var data in statesData)
        {
            data.ApplyState(data.level);
        }
    }

    public void SaveOptionData()
    {
        string data = JsonUtility.ToJson(optionData, true);
        File.WriteAllText(path + optionFileName, data);
    }

    public void LoadOptionData()
    {
        if (!File.Exists(path + optionFileName))
        {
            optionData = new OptionData();
            return;
        }

        Debug.Log("옵션 데이터를 불러왔습니다.");
        string data = File.ReadAllText(path + optionFileName);
        optionData = JsonUtility.FromJson<OptionData>(data);
    }
}
