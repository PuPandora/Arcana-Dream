using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType { Melee, Range }
    public int damage;
    public int penetrate;
    public float speed;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Initialize()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 무한 관통력
        if (penetrate <= -1) return;


        if (collision.CompareTag("Enemy"))
        {
            penetrate--;
        }

        if (penetrate <= 0)
        {
            ResetToDefault();
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ViewArea"))
        {
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
