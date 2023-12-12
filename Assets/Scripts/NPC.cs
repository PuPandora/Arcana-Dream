using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NPC : MonoBehaviour
{
    [Title("Move")]
    public Vector3 moveVec;
    public float speed = 5f;
    public bool isMoving;
    public bool isRunning;
    public bool originalDirection;

    [Title("Info")]
    public TalkData talkData;
    public SpeakerData speakerData;
    [SerializeField]
    public TalkZone talkZone;
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

        backLight.color = spriter.sharedMaterial.GetColor("_Color");
        speakerData.baseColor = spriter.sharedMaterial.GetColor("_Color");
        speakerData.eyeColor = spriter.sharedMaterial.GetColor("_EyeColor");
        speakerData.dark = spriter.sharedMaterial.GetFloat("_Dark");
    }

    void Update()
    {
        if (moveVec.magnitude != 0)
        {
            spriter.flipX = moveVec.x < 0;
        }
    }

    void LateUpdate()
    {
        spriter.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    [ContextMenu("Move")]
    public void Move(Vector3 pos)
    {
        moveRoutine = StartCoroutine(MoveRoutine(pos));
    }

    [ContextMenu("Run")]
    public void Run()
    {
        runRoutine = StartCoroutine(RunRoutine());
    }

    private IEnumerator MoveRoutine(Vector3 pos)
    {
        isMoving = true;
        anim.SetBool("IsMove", true);
        moveVec = (pos - transform.position).normalized;
        bool isGoal = false;

        FootStepSoundRoutine();

        Debug.Log("NPC 출발");
        while (!isGoal)
        {
            Vector2 dirVec = (pos - transform.position).normalized;
            rigid.MovePosition(rigid.position + dirVec * speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, pos) < 0.2f)
            {
                isGoal = true;
                Debug.Log("NPC 도착");
            }

            yield return Utils.delayFixedUpdate;
        }

        isMoving = false;
        anim.SetBool("IsMove", false);
        moveVec = Vector2.zero;
    }

    private IEnumerator FootStepSoundRoutine()
    {
        while (isMoving)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Step);
            yield return Utils.delay0_25;
        }
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

    public void LookAtTarget()
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

    public void LookLeft()
    {
        spriter.flipX = true;
    }

    public void LookRight()
    {
        spriter.flipX = false;
    }
}
