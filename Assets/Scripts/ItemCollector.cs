using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public Inventory inventory;
    public float radius;

    CircleCollider2D coll;

    public event Action<ItemData> OnGetItem;

    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        if (GameManager.instance != null)
        {
            inventory = GameManager.instance.inventory;
        }
    }

    void Start()
    {
        coll.radius = radius;
        if (GameManager.instance.gameState == GameState.Stage)
        {
            StageManager.instance.itemCollector = this;
        }
    }

    public void ChangeRadius(float value)
    {
        radius = value;
        coll.radius = radius;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exp"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.expItem);
            var item = collision.GetComponent<ExpItem>();
            StageManager.instance.GetExp(item.data.value);
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Item"))
        {
            var item = collision.GetComponent<Item>();
            bool isGet;

            // 만약 아이템 스택이 여러개 일 경우?
            // 인벤토리에 넣을 수 있는 스택만큼의 공간이 없다면?

            isGet = inventory.AddItem(item);

            // 아이템을 인벤토리에 넣을 수 없는 경우
            if (!isGet) return;

            AudioManager.instance.PlaySfx(AudioManager.Sfx.expItem);
            OnGetItem?.Invoke(item.data);
            collision.gameObject.SetActive(false);
        }
    }
}
