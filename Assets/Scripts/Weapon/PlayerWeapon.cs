using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponProperty
{
    public enum WeaponType { Melee, Range, Gear }
    public WeaponType type;

    // Weapon Object Property
    public byte prefabId;
    [ReadOnly]
    public float timer;
    public byte count;
    public byte level;

    // Bullet Property
    public float baseDamage;
    public float baseSpeed;
    public sbyte penetrate;
    // Range Weapon Property
    public float baseFireDelay;

    // Final Calculated Stats
    public float damage
    {
        get { return baseDamage * increaseDamage * moreDamage; }
    }
    public float speed
    {
        get { return baseSpeed * increaseSpeed * moreSpeed; }
    }
    public float fireDelay
    {
        get { return baseFireDelay * increaseFireDelay * moreFireDelay; }
    }

    // Increase/More Damage Property
    public float increaseDamage = 1;
    public float increaseSpeed = 1;
    public float increaseFireDelay = 1;

    public float moreDamage = 1;
    public float moreSpeed = 1;
    public float moreFireDelay = 1;

    // Total Damage
    public double totalDamage;
}

public abstract class PlayerWeapon : MonoBehaviour
{
    public WeaponProperty property = new WeaponProperty();
    public WeaponData data;
    public Action OnStatsChanged;

    // 무기 초기화
    public virtual void InitSetting()
    {
        var playerStates = GameManager.instance.playerStates;

        // transform Init
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // Prefab ID Init
        if (data.bulletPrefab)
        {
            byte? result = GameManager.instance.poolManager.FindPrefabIndex(data.bulletPrefab);
            if (result.HasValue)
            {
                property.prefabId = (byte)result;
            }
            else
            {
                Debug.LogError($"무기 데이터의 프리팹을 PoolManager에서 찾을 수 없습니다.\n" +
                    $"프리팹 : {(data.bulletPrefab ? data.id : "Null")}",
                    gameObject);
            }
        }

        // Property Init
        property.count = data.baseCount;
        property.baseFireDelay = data.baseFireDelay;

        property.baseDamage = data.baseDamage;
        property.baseSpeed = data.baseSpeed;
        property.penetrate = data.basepenetrate;

        // Read Player States
        property.increaseDamage += playerStates.damageState.totalIncreaseValue * 0.01f;
        property.moreDamage += playerStates.damageState.totalMoreValue * 0.01f;
        // + Speed
        // + Fire Delay

        property.level++;
            
        NewWeaponApplyGear();

        GameManager.instance.player.weapons.Add(this);
    }

    // 무기 레벨업
    public abstract void LevelUp(LevelUpState levelUpState);

    // 무기 사용
    public abstract void Using();

    // 장비 효과 적용
    // 중간에 새 장비로 생긴 경우
    public virtual void NewWeaponApplyGear()
    {
        if (property.type != WeaponProperty.WeaponType.Gear)
        {
            foreach (var gear in GameManager.instance.player.weapons)
            {
                if (gear.property.type == WeaponProperty.WeaponType.Gear)
                {
                    property.increaseDamage *= gear.property.increaseDamage;
                    property.increaseSpeed *= gear.property.increaseSpeed;
                    property.increaseFireDelay *= gear.property.increaseFireDelay;

                    property.moreDamage *= gear.property.moreDamage;
                    property.moreSpeed *= gear.property.moreSpeed;
                    property.moreFireDelay *= gear.property.moreFireDelay;

                    OnStatsChanged?.Invoke();
                }
            }
        }
    }

    // 기존 장비에서 Gear 아이템으로 인한 스탯 증가를 위한 Setter
    public void WeaponAddMoreDamage(float value)
    {
        if (property.type != WeaponProperty.WeaponType.Gear)
        {
            property.moreDamage += value;
            OnStatsChanged?.Invoke();
        }
    }
}
