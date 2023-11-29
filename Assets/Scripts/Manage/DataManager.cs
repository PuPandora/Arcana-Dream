using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int testInt;
    public float testFloat;
    public Inventory inventory;
}

public class DataManager : MonoBehaviour
{
    readonly string path = Application.dataPath + "/Data/SaveData/";
    string mainPath;
    string inventoryPath;

    GameManager gameManager;
    SaveData saveData;

    void Start()
    {
        mainPath = Path.Combine(path, "MainData.json");
        inventoryPath = Path.Combine(path, "InventoryData.json");
        Debug.Log($"path : {path}");

        saveData = new SaveData();

        gameManager = GameManager.instance;
        gameManager.dataManager = this;
    }

    // 추후 비동기 방식으로 구현 예정
    [ContextMenu("Save Game")]
    public void SaveGame()
    {
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
        Debug.Log("Load Game");

        if (!File.Exists(mainPath))
        {
            Debug.Log("데이터가 없습니다.");
            return;
        }

        string loadJson = File.ReadAllText(mainPath);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);

        gameManager.inventory = saveData.inventory;
        LoadInventoryData();
        PrintTestVar();
    }

    private void SaveInventoryData()
    {
        saveData.inventory = gameManager.inventory;
        string json = JsonUtility.ToJson(saveData.inventory, true);
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
        JsonUtility.FromJsonOverwrite(inventoryJson, gameManager.inventory);
    }

    private void PrintTestVar()
    {
        Debug.Log(saveData.testInt);
        Debug.Log(saveData.testFloat);
    }
}
