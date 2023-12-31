using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    [HideInInspector]
    public Scanner scanner;
    public List<PlayerWeapon> weapons;
    public bool canMove = true;

    public Vector2 moveInput { get; private set; }
    public Zone currentZone;

    public float stepInterval = 0.3f;
    public float nextStepTime = 0;

    Rigidbody2D rigid;
    Collider2D coll;
    public SpriteRenderer spriter { get; private set; }
    Animator anim;
    BoxCollider2D viewArea;
    Spawner spawner;

    private bool isLive = true;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        viewArea = GetComponentInChildren<BoxCollider2D>();
        spawner = GetComponentInChildren<Spawner>();

        isLive = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.player = this;
            GameManager.instance.viewArea = viewArea;
        }
    }

    void Start()
    {
        // StageManager는 Stage 씬에만 존재
        if (GameManager.instance.gameState == GameState.Stage)
        {
            StageManager.instance.player = this;
            StageManager.instance.spawner = spawner;
            StageManager.instance.OnGameClear += Win;
            StageManager.instance.OnGameOver += Die;
        }

        if (TalkManager.instance != null)
        {
            TalkManager.instance.OnTalkStart += LookAtTarget;
        }
    }

    void Update()
    {
        if (!isLive) return;

        if (moveInput.magnitude != 0)
        {
            nextStepTime += Time.deltaTime;
            if (nextStepTime > stepInterval)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Step);
                nextStepTime = 0f;
            }
        }
        else
        {
            nextStepTime = 0;
        }

        // 플레이어가 최근 이동항 방향을 바라봄
        if (moveInput.x != 0)
        {
            spriter.flipX = moveInput.x <= -0.05f;
        }
    }

    void LateUpdate()
    {
        if (!isLive) return;

        // 왜인지 모르겠으나 수직 이동 시 느려짐
        //transform.position = new Vector3(rigid.position.x, rigid.position.y, rigid.position.y * 0.0001f);
        spriter.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    void FixedUpdate()
    {
        if (!isLive) return;

        rigid.MovePosition(rigid.position + moveInput.normalized * speed * Time.fixedDeltaTime);

    }

    private void OnMove(InputValue value)
    {
        if (!canMove) return;

        moveInput = value.Get<Vector2>();
        anim.SetFloat("Speed", moveInput.magnitude);
    }

    private void OnInteract(InputValue value)
    {
        if (currentZone == null) return;

        currentZone.Interact();
    }

    public void Win()
    {
        anim.SetTrigger("Win");

        isLive = false;
    }

    public void Die()
    {
        anim.SetTrigger("Die");

        rigid.velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y, 5f);
        isLive = false;
        coll.enabled = false;
    }

    public void AnimationDie()
    {
        anim.SetTrigger("Die");
    }

    public void LookAtTarget()
    {
        Vector2 dirVec = TalkManager.instance.speakerPos - transform.position;
        spriter.flipX = dirVec.x < 0;
    }

    public void WakeUp()
    {
        anim.SetTrigger("WakeUp");
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;

        if (!canMove)
        {
            moveInput = Vector2.zero;
            anim.SetFloat("Speed", 0);
        }
    }
}
