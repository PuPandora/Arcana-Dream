using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType : byte { Enemy, MeleeBullet, RangeBullet, ExpItem, DropItem, DamageText };

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;
    private List<GameObject>[] poolList;

    void Awake()
    {
        // 풀 리스트 초기화
        InitializePool(out poolList, prefabs);
    }

    void Start()
    {
        GameManager.instance.poolManager = this;
    }

    private void InitializePool(out List<GameObject>[] pool, GameObject[] prefabs)
    {
        pool = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = new List<GameObject>();
        }
    }

    public GameObject Get(PoolType type)
    {
        byte index = (byte)type;
        GameObject select = null;

        // 사용 가능한 오브젝트 탐색
        foreach (var item in poolList[index])
        {
            // 있다면 반환
            if (!item.activeSelf)
            {
                item.SetActive(true);
                select = item;
                break;
            }
        }

        // 없다면 새로 만든다
        if (!select)
        {
            var item = Instantiate(prefabs[index], transform);
            poolList[index].Add(item);
            select = item;
        }

        return select;
    }

    /// <summary>
    /// PoolManager의 프리팹 배열에서 동일한 프리팹이 있는지 확인합니다.<br></br>
    /// 동일한 것이 있다면 해당 인덱스를 반환하며 없다면 null을 반환합니다.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public sbyte? FindPrefabIndex(GameObject prefab)
    {
        for (sbyte i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] == prefab)
            {
                return i;
            }
        }

        return null;
    }
}
