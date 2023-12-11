using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : MonoBehaviour
{
    SpriteRenderer spriter;
    public ItemData data;
    public byte stack;

    // 로비 아이템 테스트용
    public bool isTestItem;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();

        if (isTestItem)
        {
            Initialize(data);
        }
    }

    public void Initialize(ItemData data, byte amount = 1)
    {
        this.data = data;
        spriter.sprite = data.sprite;

        // 추후 2 이상 스택 드랍도 가능하도록 기능 추가 예정
        stack = amount;
    }

    public IEnumerator MoveToPlayerRoutine()
    {
        while (true)
        {
            Vector2 dirVec = StageManager.instance.player.transform.position - transform.position;
            transform.Translate(dirVec.normalized * 15f * Time.fixedDeltaTime);
            yield return Utils.delayFixedUpdate;
        }
    }
}