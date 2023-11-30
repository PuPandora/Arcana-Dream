using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    // Inventory
    public byte count = Utils.inventorySlotCount;
    public short[] itemIds = new short[Utils.inventorySlotCount];
    public byte[] stacks = new byte[Utils.inventorySlotCount];
    public bool[] isEmpty = new bool[Utils.inventorySlotCount];

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
        for (int i = 0; i < Utils.inventorySlotCount; i++)
        {
            Debug.Log($"Item ID : {itemIds[i]}\nStack : {stacks[i]}");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public int testInt;
    public float testFloat;
    public InventoryData inventoryData;
}

public class DataManager : MonoBehaviour
{
    // 항상, 하나만 존재하며
    // GameManager 외 다른 클래스에서 접근을 막기 위한 싱글톤
    private static DataManager instance;

    readonly string path = Application.dataPath + "/Data/SaveData/";
    string mainPath;
    string inventoryPath;

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
        mainPath = Path.Combine(path, "MainData.json");
        inventoryPath = Path.Combine(path, "InventoryData.json");
        Debug.Log($"Save Data Path : {path}");

        saveData = new SaveData();

        gameManager = GameManager.instance;
        gameManager.dataManager = this;
    }

    // 추후 비동기 방식으로 구현 예정
    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        Debug.Log("데이터 저장");
        Debug.Log("Save Game");
        saveData = new SaveData();

        SaveInventoryData();
        saveData.testInt = Random.Range(1, 100);
        saveData.testFloat = Random.Range(1f, 10f);

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(mainPath, json);
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        Debug.Log("데이터 불러오기");
        Debug.Log("Load Game");

        if (!File.Exists(mainPath))
        {
            Debug.Log("데이터가 없습니다.");
            return;
        }

        string loadJson = File.ReadAllText(mainPath);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        LoadInventoryData();
        PrintTestVar();
    }

    private void SaveInventoryData()
    {
        saveData.inventoryData = gameManager.inventory.GetInventoryData();
        string json = JsonUtility.ToJson(saveData.inventoryData, true);

        File.WriteAllText(inventoryPath, json);
    }

    private void LoadInventoryData()
    {
        if (!File.Exists(inventoryPath))
        {
            Debug.Log("인벤토리 데이터가 없습니다.");
            return;
        }

        string inventoryJson = File.ReadAllText(inventoryPath);
        JsonUtility.FromJsonOverwrite(inventoryJson, saveData.inventoryData);

        gameManager.inventory.ApplyData(saveData.inventoryData);
    }

    private void PrintTestVar()
    {
        Debug.Log(saveData.testInt);
        Debug.Log(saveData.testFloat);
    }
}
