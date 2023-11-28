using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public Scanner scanner;
    public List<Weapon> weapons;

    public Vector2 moveInput { get; private set; }

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    BoxCollider2D viewArea;
    Spawner spawner;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        viewArea = GetComponentInChildren<BoxCollider2D>();
        spawner = GetComponentInChildren<Spawner>();
    }

    void Start()
    {
        GameManager.instance.player = this;
        GameManager.instance.viewArea = viewArea;
        GameManager.instance.spawner = spawner;
    }

    void Update()
    {
        if (moveInput.x != 0)
        {
            spriter.flipX = moveInput.x <= -0.05f;
        }
    }

    void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveInput.normalized * speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        anim.SetFloat("Speed", moveInput.magnitude);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 임시 대미지 피격 코드
            GameManager.instance.GetDamaged(5 * Time.deltaTime);
        }
    }
}
