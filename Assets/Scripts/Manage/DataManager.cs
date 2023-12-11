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
    public long gold = 0;
    public Vector3 position;
    public bool playerSpriteFlip;
    public int playedStageCount = 0;
    public string lastPlayedDate = "";
    public byte tutorialIndex = 0;
    public bool isNewGame = true;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public byte saveSlotIndex = 0;
    public string path { get; private set; }
    public string saveFileName { get; private set; } = "SaveData";
    public string optionFileName { get; private set; } = "OptionData";

    public SaveData saveData = new SaveData();
    public OptionData optionData;

    void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        #endregion
        path = Application.persistentDataPath + "/";
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

        saveData.playedStageCount = GameManager.instance.playedStageCount;
        saveData.isNewGame = GameManager.instance.isNewGame;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path + saveFileName + saveSlotIndex + ".json", json);

        Debug.Log($"게임 데이터 저장 완료\n경로 : {path}");
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        var saveFilePath = path + saveFileName + saveSlotIndex + ".json";
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("데이터가 없습니다.");
            return;
        }

        string loadJson = File.ReadAllText(saveFilePath);
        JsonUtility.FromJsonOverwrite(loadJson, saveData);

        GameManager.instance.inventory.ApplyData(saveData.inventoryData);
        GameManager.instance.SetGold(saveData.gold);
        GameManager.instance.player.transform.position = saveData.position;
        GameManager.instance.playerCam.transform.position = saveData.position;
        GameManager.instance.player.spriter.flipX = saveData.playerSpriteFlip;

        GameManager.instance.playedStageCount = saveData.playedStageCount;
        GameManager.instance.isNewGame = saveData.isNewGame;

        LoadStatesData();

        Debug.Log("게임 데이터 불러오기 완료");
    }

    /// <summary>
    /// 세이브 슬롯용 데이터 불러오는 메소드
    /// </summary>
    public SaveData LoadSaveData(byte index)
    {
        SaveData data = new SaveData();
        string loadJson = File.ReadAllText(path + saveFileName + index + ".json");
        JsonUtility.FromJsonOverwrite(loadJson, data);

        return data;
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
        SaveJsonData(path + optionFileName, data);
    }

    public void LoadOptionData()
    {
        if (!CheckJsonFileExist(path + optionFileName))
        {
            optionData = new OptionData();
            SaveOptionData();
            return;
        }

        string data = LoadJsonData(path + optionFileName);
        optionData = JsonUtility.FromJson<OptionData>(data);
    }

    private bool CheckJsonFileExist(string path)
    {
        return File.Exists(path + ".json");
    }

    private void SaveJsonData(string path, string data)
    {
        File.WriteAllText(path + ".json", data);
    }

    private string LoadJsonData(string path)
    {
        return File.ReadAllText(path + ".json");
    }
}
