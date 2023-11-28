using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCard : PlayerWeapon
{
    private List<Bullet> bullets;

    public override void InitSetting()
    {
        base.InitSetting();
        bullets = new List<Bullet>();
        property.type = WeaponProperty.WeaponType.Melee;
        OnStatsChanged += InitalizeBullets;

        ReloadBullet(property.count);
        Debug.Log("회전 카드 Init Setting");
    }

    public override void LevelUp(LevelUpState levelUpState)
    {
        property.level++;
        Debug.Log($"회전 카드 레벨업.\n현재 레벨 : {property.level}");
        property.increaseDamage += levelUpState.damage;
        property.increaseSpeed += levelUpState.speed;

        property.count += levelUpState.count;
        if (levelUpState.count > 0)
        {
            ReloadBullet(property.count);
        }
    }

    private void ReloadBullet(byte count)
    {
        for (byte i = 0; i < count; i++)
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
                bulletTransform.parent = transform;

                var instantBullet = bulletTransform.GetComponent<Bullet>();
                instantBullet.penetrate = -1;
                bullets.Add(instantBullet);
            }

            // 초기화
            bulletTransform.parent = transform;
            bulletTransform.position = transform.position;
            bulletTransform.rotation = Quaternion.identity;

            // 개수에 맞는 원형 회전 값 구하기
            Vector3 rotVec = Vector3.forward * 360 * i / property.count;

            // 위치 재조정
            bulletTransform.Rotate(rotVec);
            bulletTransform.Translate(Vector3.up * 1.5f);

            InitalizeBullets();
        }
    }

    public void InitalizeBullets()
    {
        // 속성 초기화
        foreach (var bullet in bullets)
        {
            bullet.Initialize(property.damage, property.penetrate, property.speed);
        }
    }

    public override void Using()
    {
        RotateWeapon();
    }

    private void RotateWeapon()
    {
        transform.Rotate(Vector3.forward * property.speed * Time.deltaTime);
    }

    void OnDestroy()
    {
        OnStatsChanged -= InitalizeBullets;
    }
}
