using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee, Range }
    [EnumToggleButtons]
    public WeaponType type;

    public byte bulletId;
    public float speed;
    public sbyte penetrate;

    public float timer;
    public float fireDelay;

    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        Rotate();
        Fire();
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

    private void Fire()
    {
        if (type != WeaponType.Range) return;

        timer += Time.deltaTime;
        bool canFire = (player.scanner.nearestTarget != null) && (timer >= fireDelay);

        if (canFire)
        {
            timer = 0f;

            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 myPos = transform.position;
            Vector3 dirVec = (targetPos - myPos).normalized;

            GameObject instanceBullet = GameManager.instance.poolManager.Get(PoolType.RangeBullet);
            Rigidbody2D bulletRigid = instanceBullet.GetComponent<Rigidbody2D>();
            Bullet bulletScript = instanceBullet.GetComponent<Bullet>();

            instanceBullet.transform.position = transform.position;
            instanceBullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dirVec);
            bulletRigid.velocity = dirVec * bulletScript.speed;
            bulletScript.penetrate = penetrate;
        }
    }
}