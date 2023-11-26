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
}
