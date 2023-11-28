using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public Inventory inventory;
    public float radius;

    CircleCollider2D coll;

    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        coll.radius = radius;
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
            var item = collision.GetComponent<ExpItem>();
            StageManager.instance.GetExp(item.expVaule);
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Item"))
        {
            var item = collision.GetComponent<Item>();
            bool isGet;

            isGet = inventory.AddItem(item);

            if (!isGet) return;

            collision.gameObject.SetActive(false);
        }
    }
}
