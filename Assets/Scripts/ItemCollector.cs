using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
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
        if (collision.CompareTag("Item"))
        {
            var item = collision.GetComponent<Item>();

            switch (item.type)
            {
                case ItemType.Exp:
                    GameManager.instance.GetExp(item.value);
                    break;
                case ItemType.Item:
                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }
}
