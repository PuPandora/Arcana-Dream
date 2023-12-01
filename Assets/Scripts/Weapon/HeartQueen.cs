using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartQueen : PlayerWeapon
{
    public override void InitSetting()
    {
        base.InitSetting();
        property.type = WeaponProperty.WeaponType.Gear;
        Debug.Log("하트 퀸 Init Setting");
    }

    public override void LevelUp(LevelUpState levelUpState)
    {
        property.level++;
        Debug.Log($"하트 퀸 레벨업.\n현재 레벨 : {property.level}");

        property.moreDamage += levelUpState.damage;

        foreach (var item in GameManager.instance.player.weapons)
        {
            item.WeaponAddMoreDamage(levelUpState.damage);
        }
    }

    public override void Using()
    {
        // 플레이어를 따라다니는 하트 퀸 오브젝트 (파티클 & 쉐이더로 이펙트)
    }
}
