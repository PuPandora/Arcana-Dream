using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee, Range }
    [EnumToggleButtons]
    public WeaponType type;

    // Weapon Object Property
    public byte prefabId;
    [ReadOnly]
    public float timer;
    public byte count;

    // Bullet Property
    public float damage;
    public float speed;
    public sbyte penetrate;

    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        Rotate();
        Fire();

        // Test Code
        if (Input.GetButtonDown("Jump"))
        {
            AddMeleeWeapon();
        }
    }

    private void Rotate()
    {
        switch (type)
        {
            case WeaponType.Melee:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            case WeaponType.Range:
                break;
        }
    }

    private void AddMeleeWeapon()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bulletTransform;

            // 이미 있는 것을 활용
            if (i < transform.childCount)
            {
                bulletTransform = transform.GetChild(i);
            }
            // 부족하다면 생성
            else
            {
                bulletTransform = GameManager.instance.poolManager.Get(PoolType.MeleeBullet).transform;
            }

            // 초기화
            bulletTransform.parent = transform;
            bulletTransform.position = transform.position;
            bulletTransform.rotation = Quaternion.identity;

            // 개수에 맞는 원형 회전 값 구하기
            Vector3 rotVec = Vector3.forward * 360 * i / count;

            // 위치 재조정
            bulletTransform.Rotate(rotVec);
            bulletTransform.Translate(Vector3.up * 1.5f);
        }

        count++;
    }

    private void Fire()
    {
        if (type != WeaponType.Range) return;

        timer += Time.deltaTime;
        bool canFire = (player.scanner.nearestTarget != null) && (timer >= speed);

        if (canFire)
        {
            timer = 0f;

            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 myPos = transform.position;
            Vector3 dirVec = (targetPos - myPos).normalized;

            GameObject instanceBullet = GameManager.instance.poolManager.Get(PoolType.RangeBullet);
            Bullet bulletScript = instanceBullet.GetComponent<Bullet>();
            Rigidbody2D bulletRigid = bulletScript.rigid;

            instanceBullet.transform.position = transform.position;
            instanceBullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dirVec);
            bulletRigid.velocity = dirVec * bulletScript.speed;
            bulletScript.penetrate = penetrate;
        }
    }
}