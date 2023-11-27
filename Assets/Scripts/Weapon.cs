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

    // Range Weapon Property
    public float fireDelay;

    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    public void Initialize(WeaponData data)
    {
        type = data.type;

        sbyte? result = GameManager.instance.poolManager.FindPrefabIndex(data.bulletPrefab);
        if (result != null)
        {
            prefabId = (byte)result;
        }

        count = data.count;
        fireDelay = data.fireDelay;

        damage = data.damage;
        speed = data.speed;
        penetrate = data.penetrate;

        if (type == WeaponType.Melee)
        {
            AddMeleeWeapon();
        }
    }

    void Update()
    {
        Using();

        // Test Code
        if (Input.GetButtonDown("Jump") && type == WeaponType.Melee)
        {
            AddMeleeWeapon();
        }
    }

    private void Using()
    {
        switch (type)
        {
            case WeaponType.Melee:
                RotateWeapon();
                break;
            case WeaponType.Range:
                Fire();
                break;
        }
    }

    private void RotateWeapon()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
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
        timer += Time.deltaTime;
        bool canFire = (player.scanner.nearestTarget != null) && (timer >= fireDelay);

        if (canFire)
        {
            timer = 0f;

            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 myPos = transform.position;
            Vector3 dirVec = (targetPos - myPos).normalized;

            GameObject bullet = GameManager.instance.poolManager.Get(PoolType.RangeBullet);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dirVec);
            bulletScript.Initialize(damage, penetrate, speed, dirVec);
        }
    }
}