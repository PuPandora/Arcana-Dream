using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType { Melee, Range }
    public BulletType type;
    public float damage;
    public sbyte penetrate;
    public float speed;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float damage, sbyte penetrate, float speed, Vector3 dirVec)
    {
        this.damage = damage;
        this.penetrate = penetrate;
        this.speed = speed;
        rigid.velocity = dirVec * speed;
    }

    public void Initialize(float damage, sbyte penetrate, float speed)
    {
        this.damage = damage;
        this.penetrate = penetrate;
        this.speed = speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 무한 관통력
        if (penetrate <= -1) return;

        if (collision.CompareTag("Enemy"))
        {
            penetrate--;

            if (penetrate < 0)
            {
                ResetToDefault();
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ViewArea"))
        {
            if (penetrate <= -1) return;

            ResetToDefault();
            gameObject.SetActive(false);
        }
    }

    private void ResetToDefault()
    {
        rigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
