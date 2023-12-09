using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NPC : MonoBehaviour
{
    [Title("Move")]
    public Vector3 originalPos;
    public Vector3 moveVec;
    public float speed = 5f;
    public bool isRunning;
    public bool originalDirection;

    [Title("Info")]
    public TalkData talkData;
    [SerializeField]
    TalkZone talkZone;
    public bool canTalk = true;
    public bool isShopNpc;
    public LobbyUI shopUI;

    [Title("Components")]
    public Light2D backLight;
    Coroutine moveRoutine;
    Coroutine runRoutine;
    Rigidbody2D rigid;
    public SpriteRenderer spriter { get; private set; }
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalPos = transform.position;

        backLight.color = spriter.sharedMaterial.GetColor("_Color");
        originalDirection = spriter.flipX;
    }

    void Start()
    {
        TalkManager.instance.OnTalkStart += LookAtTarget;
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
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.0001f);
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

    private void LookAtTarget()
    {
        Vector2 dirVec = GameManager.instance.player.transform.position - transform.position;
        spriter.flipX = dirVec.x < 0f;
    }

    public IEnumerator ResetLook()
    {
        yield return Utils.delay1;
        spriter.flipX = originalDirection;
    }

    public void SetCanTalk(bool canTalk)
    {
        this.canTalk = canTalk;
        talkZone.zone.enabled = canTalk;
    }
}
