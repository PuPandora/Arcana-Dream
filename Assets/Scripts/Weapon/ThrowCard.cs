using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ThrowCard : PlayerWeapon
{
    private Player player;

    public override void InitSetting()
    {
        base.InitSetting();
        property.type = WeaponProperty.WeaponType.Range;
        player = GameManager.instance.player;
        Debug.Log("투척 카드 Init Setting");
    }

    public override void LevelUp(LevelUpState levelUpState)
    {
        property.level++;
        Debug.Log($"투척 카드 레벨업.\n현재 레벨 : {property.level}");
        property.increaseDamage += levelUpState.damage;
        property.increaseSpeed += levelUpState.speed;
        property.increaseFireDelay -= levelUpState.fireDelay;
    }

    public override void Using()
    {
        ThrowWeapon();
    }

    private void ThrowWeapon()
    {
        property.timer += Time.deltaTime;
        bool canFire = (player.scanner.nearestTarget != null) && (property.timer >= property.fireDelay);

        if (canFire)
        {
            property.timer = 0f;

            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 myPos = transform.position;
            Vector3 dirVec = (targetPos - myPos).normalized;

            GameObject bullet = GameManager.instance.poolManager.Get(PoolType.RangeBullet);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dirVec);
            bulletScript.Initialize(this, property.damage, property.penetrate, property.speed, dirVec);
        }
    }
}
