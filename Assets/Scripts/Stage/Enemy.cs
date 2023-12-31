using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [ReadOnly]
    private EnemyData data;
    public Transform target;
    public DropItem dropItem;

    [ReadOnly]
    private float health;
    public bool isLive { get; private set; } = true;
    private Vector2 dirVec;

    private bool isDamaged;
    Coroutine knockBackRoutine;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    CapsuleCollider2D coll;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        dropItem = GetComponent<DropItem>();

        anim.SetBool("IsChase", true);
        StageManager.instance.OnGameOver += InvokeMoveAway;
    }

    public void Initalize(EnemyData data)
    {
        this.data = data;
        health = data.maxHealth;

        anim.runtimeAnimatorController = data.animController;
        coll.offset = data.collOffset;
        coll.direction = data.collDirection;
        coll.size = data.collSize;

        anim.SetBool("IsChase", true);
    }

    void OnEnable()
    {
        isLive = true;
        coll.enabled = true;
        isDamaged = false;

        if (target == null)
        {
            target = StageManager.instance.player.transform;
        }
        StartCoroutine(CalculateDirectionRoutine());
    }


    void Update()
    {
        if (!isLive) return;
    }

    void FixedUpdate()
    {
        if (isDamaged) return;

        ChaseTarget();
    }

    private void ChaseTarget()
    {
        rigid.MovePosition(rigid.position + dirVec.normalized * data.speed * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        if (!isLive) return;

        spriter.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    private void InvokeMoveAway()
    {
        anim.SetBool("IsChase", false);
        target = null;
        dirVec = Vector2.zero;
        Invoke(nameof(MoveAway), Random.Range(1f, 5f));
    }

    private void MoveAway()
    {
        anim.SetBool("IsChase", true);
        dirVec = (transform.position - StageManager.instance.player.transform.position).normalized;
        spriter.flipX = dirVec.x < 0;
        GetComponent<Reposition>().enabled = false;
        Destroy(gameObject, 10f);
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
        bullet.Attack();

        if (health <= 0)
        {
            Die();
        }
        else
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);

            if (knockBackRoutine != null)
            {
                StopCoroutine(knockBackRoutine);
            }

            knockBackRoutine = StartCoroutine(KnockBack());
        }
    }

    private IEnumerator KnockBack()
    {
        isDamaged = true;
        Vector2 dirVec = transform.position - target.position;
        rigid.velocity = dirVec.normalized * 1f;

        yield return new WaitForSeconds(0.15f);

        rigid.velocity = Vector2.zero;
        isDamaged = false;
    }

    public void Hit(float value)
    {
        DisplayDamageText(value);
        health -= value;

        if (health <= 0)
        {
            Die(false, false, false);
        }
    }

    private void DisplayDamageText(float value)
    {
        GameObject tmp = GameManager.instance.poolManager.Get(PoolType.DamageText);
        // 너무 많은 GetComponent 호출, 최적화 필요
        // Enemy마다 대미지 텍스트 컴포넌트 배열을 가지고
        // 필요할 때 그걸로 호출?
        var damageText = tmp.GetComponent<DamageText>();

        damageText.gameObject.transform.position = transform.position + Vector3.up;
        damageText.SetText(value);
    }

    private void Die(bool addKillCount = true, bool dropExp = true, bool dropItem = true)
    {
        if (!isLive) return;

        isLive = false;
        isDamaged = false;

        if (addKillCount)
        {
            StageManager.instance.AddKillCount();
        }
        if (dropExp)
        {
            DropExpItem();
        }
        if (dropItem)
        {
            DropItem();
        }

        if (StageManager.instance.isPlaying)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }

        coll.enabled = false;
        gameObject.SetActive(false);
    }

    private void DropExpItem()
    {
        var item = GameManager.instance.poolManager.Get(PoolType.ExpItem);
        ExpItem expItem = item.GetComponent<ExpItem>();

        expItem.transform.position = transform.position;
        expItem.transform.rotation = Quaternion.identity;
        expItem.Initialize(data.expItemData);
    }

    private void DropItem()
    {
        for (int i = 0; i < data.itemDropTable.Length; i++)
        {
            float randNum = Random.Range(0f, 1f);
            if (data.itemDropTable[i].itemDropRate < randNum) continue;

            var item = GameManager.instance.poolManager.Get(PoolType.DropItem);
            Item dropItem = item.GetComponent<Item>();

            dropItem.transform.position = transform.position;
            dropItem.transform.rotation = Quaternion.identity;
            dropItem.Initialize(data.itemDropTable[i].itemData);
        }
    }

    private IEnumerator CalculateDirectionRoutine()
    {
        while (isLive && target)
        {
            dirVec = target.position - transform.position;
            spriter.flipX = dirVec.normalized.x < 0 ? true : false;
            yield return Utils.delay0_25;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform == target)
        {
            // 임시 대미지 피격 코드
            StageManager.instance.GetDamaged(data.damage * Time.deltaTime);
        }
    }

    void OnDestroy()
    {
        StageManager.instance.OnGameOver -= InvokeMoveAway;
    }
}
