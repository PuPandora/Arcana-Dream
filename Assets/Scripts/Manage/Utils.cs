using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Utils : MonoBehaviour
{
    //public static WaitForFixedUpdate waitForFixedUpdate;
    public static WaitForSeconds delay0_1 = new WaitForSeconds(0.1f);
    public static WaitForSeconds delay0_25 = new WaitForSeconds(0.25f);

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
        // 데이터가 많아지면 아이템 데이터 정렬 + 이진 검색
        for (short i = 0; i < GameManager.instance.itemDataBase.Length; i++)
        {
            if (id != GameManager.instance.itemDataBase[i].id) continue;

            return i;
        }

        Debug.LogWarning("아이템 데이터에서 인덱스 검색 실패");
        return -1;
    }
}
