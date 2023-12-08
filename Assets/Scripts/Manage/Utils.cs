using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class Utils : MonoBehaviour
{
    readonly public static WaitForSeconds delay0_05 = new WaitForSeconds(0.05f);
    readonly public static WaitForSeconds delay0_1 = new WaitForSeconds(0.1f);
    readonly public static WaitForSeconds delay0_25 = new WaitForSeconds(0.25f);
    readonly public static WaitForSeconds delay1 = new WaitForSeconds(1f);
    readonly public static WaitForSeconds delay2 = new WaitForSeconds(2f);
    readonly public static WaitForFixedUpdate delayFixedUpdate;

    public const byte INVENTORY_SLOT_COUNT = 30;
    public const byte SAVE_DATA_SLOT_COUNT = 4;

    public static Vector2 GetRandomVector(float min, float max)
    {
        return new Vector2(Random.Range(min, max), Random.Range(min, max));
    }

    public static Vector2 GetRandomVector(float minX, float maxX, float minY, float maxY)
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    #region 데이터 관련
    /// <summary>
    /// 아이템 데이터 베이스에서 id로 아이템이 있는지 탐색합니다.
    /// </summary>
    /// <param name="id">아이템의 id</param>
    /// <returns>id와 같은 아이템이 없다면 null을 반환합니다.</returns>
    public static ItemData GetItemDataWithId(short id)
    {
        short index = GetItemDataIndex(id);
        return index != -1 ? GameManager.instance.itemDataBase[index] : null;
    }

    private static short GetItemDataIndex(short id)
    {
        // 임시 선형 검색
        // 데이터가 많아지면 이진 검색
        for (short i = 0; i < GameManager.instance.itemDataBase.Length; i++)
        {
            if (id != GameManager.instance.itemDataBase[i].id) continue;

            return i;
        }

        Debug.LogWarning($"아이템 데이터 베이스에서 인덱스 검색 실패\n시도한 ID : {id}");
        return -1;
    }

    /// <summary>
    /// 적 데이터 베이스에서 id로 아이템이 있는지 탐색합니다.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>id와 같은 적 데이터가 없다면 null을 반환합니다.</returns>
    public static EnemyData GetEnemyDataWithId(short id)
    {
        short index = GetEnemyDataIndex(id);
        return index != -1 ? GameManager.instance.enemyDataBase[index] : null;
    }

    private static short GetEnemyDataIndex(short id)
    {
        // 임시 선형 검색
        // 데이터가 많아지면 이진 검색
        for (short i = 0; i < GameManager.instance.enemyDataBase.Length; i++)
        {
            Debug.Log($"GameManager.instance.enemyDataBase[i].id : {GameManager.instance.enemyDataBase[i].id}\n" +
                $"시도하는 ID : {id}");
            
            if (id != GameManager.instance.enemyDataBase[i].id) continue;

            return i;
        }

        Debug.LogWarning($"적 데이터 베이스에서 인덱스 검색 실패\n시도한 ID : {id}");
        return -1;
    }

    /// <summary>
    /// Data의 id를 비교하여 오름차순으로 정렬합니다.
    /// </summary>
    public static int CompareDataId(GameObjectData data1, GameObjectData data2)
    {
        return data1.id.CompareTo(data2.id);
    }
    #endregion

    /// <summary>
    /// StageData를 JSON 파일로 저장합니다
    /// </summary>
    public static void SaveStageData(StageData data)
    {
        // 경로 정하기
        string path = Path.Combine(Application.dataPath + $"/Data/StageData/NewStageData.json");
        short fileIndex = -1;
        if (File.Exists(path))
        {
            do
            {
                path = Path.Combine(Application.dataPath + $"/Data/StageData/NewStageData_{++fileIndex}.json");
            } while (File.Exists(path));
        }

        Debug.Log(data.name);
        Debug.Log(data.stageTable.Length);
        Debug.Log(data.stageTable[0].spawnDelay);
        Debug.Log(data.stageTable[0].spawnTable.Length);

        // StageTable의 모든 적 데이터 ID 초기화
        for (byte i = 0; i < data.stageTable.Length; i++)
        {
            for (byte j = 0; j < data.stageTable[i].spawnTable.Length; j++)
            {
                data.stageTable[i].spawnTable[j].InitEnemyId();
            }
        }

        string stageData = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, stageData);

        Debug.Log($"StageData가 저장되었습니다.\n경로 : {path}");
    }

    /// <summary>
    /// JSON 파일을 StageData를 적용시킵니다.
    /// </summary>
    public static void LoadStageData(string jsonData, StageData stageData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, stageData);

        // StageTable의 모든 적 데이터 초기화
        for (int i = 0; i < stageData.stageTable.Length; i++)
        {
            var spawnTable = stageData.stageTable[i].spawnTable;

            for (int j = 0; j < spawnTable.Length; j++)
            {
                spawnTable[j].InitEnemyData();
            }
        }
    }
}
