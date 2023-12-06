using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NPC : MonoBehaviour
{
    public Vector3 originalPos;
    public Vector3 moveVec;
    public float speed = 5f;
    public bool isRunning;

    public Light2D backLight;

    Coroutine moveRoutine;
    Coroutine runRoutine;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalPos = transform.position;

        backLight.color = spriter.sharedMaterial.GetColor("_Color");
    }

    void Update()
    {
        anim.SetFloat("Speed", moveVec.magnitude);
        anim.SetBool("IsRunning", isRunning);
        if (moveVec.magnitude != 0)
        {
            spriter.flipX = moveVec.x < 0;
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    [ContextMenu("Move")]
    public void Move()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            transform.position = originalPos;
        }

        moveRoutine = StartCoroutine(MoveRoutine());
    }

    [ContextMenu("Run")]
    public void Run()
    {
        if (runRoutine != null)
        {
            StopCoroutine(runRoutine);
            transform.position = originalPos;
        }

        runRoutine = StartCoroutine(RunRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float leftDistance = 0f;
        float rightDistance = 0f;
        

        while (leftDistance < 3f)
        {
            rigid.MovePosition(rigid.position + Vector2.left * speed * Time.fixedDeltaTime);
            moveVec = Vector2.left;
            leftDistance += Vector2.left.x * Time.fixedDeltaTime * -1;
            yield return Utils.delayFixedUpdate;
        }
        while (rightDistance < 3f)
        {
            rigid.MovePosition(rigid.position + Vector2.right * speed * Time.fixedDeltaTime);
            moveVec = Vector2.right;
            rightDistance += Vector2.right.x * Time.fixedDeltaTime;
            yield return Utils.delayFixedUpdate;
        }

        moveVec = Vector2.zero;
    }

    private IEnumerator RunRoutine()
    {
        isRunning = true;

        float leftDistance = 0f;
        float rightDistance = 0f;

        while (leftDistance < 3f)
        {
            rigid.MovePosition(rigid.position + Vector2.left * speed * 2 * Time.fixedDeltaTime);
            moveVec = Vector2.left;
            leftDistance += Vector2.left.x * Time.fixedDeltaTime * -1;
            yield return Utils.delayFixedUpdate;
        }
        while (rightDistance < 3f)
        {
            rigid.MovePosition(rigid.position + Vector2.right * speed * 2 * Time.fixedDeltaTime);
            moveVec = Vector2.right;
            rightDistance += Vector2.right.x * Time.fixedDeltaTime;
            yield return Utils.delayFixedUpdate;
        }

        isRunning = false;
        moveVec = Vector2.zero;
    }
}
