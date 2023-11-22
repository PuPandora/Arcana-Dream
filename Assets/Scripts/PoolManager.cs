using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public enum EPoolType : byte { ENEMY };
    public GameObject[] prefabs;
    List<GameObject>[] poolList;

    void Start()
    {
        // 풀 리스트 초기화
        poolList = new List<GameObject>[prefabs.Length];
        for (int i = 0;  i < prefabs.Length; i++)
        {
            poolList[i] = new List<GameObject>();
        }
    }

    public GameObject Get(EPoolType type)
    {
        byte index = (byte)type;
        GameObject select = null;

        foreach (var item in poolList[index])
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                select = item;
                break;
            }
        }

        if (select == null)
        {
            var item = Instantiate(prefabs[index], transform);
            poolList[index].Add(item);
            select = item;
        }

        return select;
    }
}
