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

    }
}
