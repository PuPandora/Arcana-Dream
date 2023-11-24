using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public Scanner scanner;
    public Weapon[] weapons;

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
        weapons = GetComponentsInChildren<Weapon>(true);
    }

    void Start()
    {
        GameManager.instance.player = this;
        GameManager.instance.viewArea = viewArea;
        GameManager.instance.spawner = spawner;
        GameManager.instance.weapons = new GameObject[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            GameManager.instance.weapons[i] = weapons[i].gameObject;
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
