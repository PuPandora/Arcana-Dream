using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public Scanner scanner;
    public List<PlayerWeapon> weapons;

    public Vector2 moveInput { get; private set; }

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    BoxCollider2D viewArea;
    Spawner spawner;

    void Awake()
    {
        Debug.Log("Player Awake");
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        viewArea = GetComponentInChildren<BoxCollider2D>();
        spawner = GetComponentInChildren<Spawner>();

        GameManager.instance.player = this;
        GameManager.instance.viewArea = viewArea;
    }

    void Start()
    {
        if (StageManager.instance != null)
        {
            StageManager.instance.player = this;
            StageManager.instance.spawner = spawner;
        }
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
}
