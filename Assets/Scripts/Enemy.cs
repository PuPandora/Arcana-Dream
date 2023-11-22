using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public int health;
    public int maxHealth;

    private bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        isLive = true;
    }

    void Update()
    {
        if (!isLive) return;

        spriter.flipX = target.position.x < transform.position.x;
    }

    void FixedUpdate()
    {
        ChaseTarget();
    }

    private void ChaseTarget()
    {
        if (!isLive || target == null) return;

        Vector2 dirVec = target.position - transform.position;
        rigid.MovePosition(rigid.position + dirVec.normalized * speed * Time.fixedDeltaTime);
    }
}
