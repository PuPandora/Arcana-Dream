using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Utils : MonoBehaviour
{
    //public static WaitForFixedUpdate waitForFixedUpdate;
    public static WaitForSeconds delay0_1 = new WaitForSeconds(0.1f);
    public static WaitForSeconds delay0_25 = new WaitForSeconds(0.25f);

    public const byte inventorySlotCount = 30;

    public static Vector2 GetRandomVector(float min, float max)
    {
        return new Vector2(Random.Range(min, max), Random.Range(min, max));
    }

    public static Vector2 GetRandomVector(float minX, float maxX, float minY, float maxY)
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    /// <summary>
    /// 아이템 데이터 베이스에서 id로 아이템이 있는지 탐색합니다. <br></br>
    /// 만약 해당 id와 같은 아이템이 없다면 null을 반환합니다.
    /// </summary>
    /// <param name="id">아이템의 id</param>
    /// <returns></returns>
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
    /// 적 데이터 베이스에서 id로 아이템이 있는지 탐색합니다. <br></br>
    /// 만약 해당 id와 같은 적 데이터가 없다면 null을 반환합니다.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static EnemyData GetEnemyDataWithId(short id)
    {
        short index = GetEnemyDataIndex(id);
        return index != -1 ? GameManager.instance.enemyDataBase[index] : null;
    }

    public static short GetEnemyDataIndex(short id)
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
    /// <param name="data1"></param>
    /// <param name="data2"></param>
    /// <returns></returns>
    public static int CompareDataId(GameObjectData data1, GameObjectData data2)
    {
        return data1.id.CompareTo(data2.id);
    }
}
