using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float health;
    public float maxHealth;
    public ExpItemData expItemData;

    public bool isLive { get; private set; } = true;
    private Vector2 dirVec;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    Collider2D coll;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    void Start()
    {
        StartCoroutine(CalculateDirectionRoutine());
    }

    public void Initalize(EnemyData data)
    {
        speed = data.speed;
        health = data.health;
        maxHealth = data.maxHealth;
        expItemData = data.expItemData;

        anim.runtimeAnimatorController = data.animController;
        data.collPreset.ApplyTo(coll);
    }

    void OnEnable()
    {
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        StartCoroutine(CalculateDirectionRoutine());
    }

    void Update()
    {
        if (!isLive) return;
        anim.SetBool("IsChase", target);

        if (!target) return;
        spriter.flipX = target.position.x < transform.position.x;
    }

    void FixedUpdate()
    {
        ChaseTarget();
    }

    private void ChaseTarget()
    {
        rigid.MovePosition(rigid.position + dirVec.normalized * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Hit(collision.GetComponent<Bullet>());
        }
    }

    private void Hit(Bullet bullet)
    {
        DisplayDamageText(bullet.damage);
        health -= bullet.damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void DisplayDamageText(float value)
    {
        GameObject tmp = GameManager.instance.poolManager.Get(PoolType.DamageText);
        // 너무 많은 GetComponent 호출, 리팩토링 필요
        var damageText = tmp.GetComponent<DamageText>();

        damageText.gameObject.transform.position = transform.position + Vector3.up;
        damageText.SetText(value);
    }

    private void Die()
    {
        isLive = false;

        GameManager.instance.AddKillCount();

        var item = GameManager.instance.poolManager.Get(PoolType.ExpItem);
        item.transform.position = transform.position;

        ExpItem expItem = item.GetComponent<ExpItem>();
        expItem.Initalize(expItemData);

        coll.enabled = false;
        gameObject.SetActive(false);
    }

    private IEnumerator CalculateDirectionRoutine()
    {
        while (isLive && target)
        {
            dirVec = target.position - transform.position;
            yield return Utils.delay0_25;
        }
    }
}
