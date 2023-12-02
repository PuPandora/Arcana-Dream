using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    // Inventory
    [HideInInspector] public byte count = Utils.INVENTORY_SLOT_COUNT;
    public short[] itemIds = new short[Utils.INVENTORY_SLOT_COUNT];
    public byte[] stacks = new byte[Utils.INVENTORY_SLOT_COUNT];
    public bool[] isEmpty = new bool[Utils.INVENTORY_SLOT_COUNT];

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

        for (int i = 0; i < isEmpty.Length; i++)
        {
            isEmpty[i] = true;
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
        gameManager.dataManager = this;
    }

    // 추후 비동기 방식으로 구현 예정
    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        saveData.inventoryData = GameManager.instance.inventory.GetInventoryData();
        saveData.playerStateData = GameManager.instance.playerStates;

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
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        Debug.Log("게임 데이터 불러오기 완료");
    }
}
