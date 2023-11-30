using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponName", menuName = "Scriptable Object/Weapon Data")]
public class WeaponData : GameObjectData
{
    [EnumToggleButtons]
    public WeaponProperty.WeaponType type;

    [Title("Weapon Property")]
    // Weapon Object Property
    public string weaponName;
    [TextArea] public string desc;
    [PreviewField] public Sprite icon;

    public byte baseCount;
    public float baseFireDelay;

    [Title("Bullet Property")]
    // Bullet Property
    public float baseDamage;
    public float baseSpeed;
    public sbyte basepenetrate;
    public GameObject bulletPrefab;

    [Title("Level")]
    public LevelUpState[] levelUpStates = new LevelUpState[6];
}

[System.Serializable]
public struct LevelUpState
{
    public float damage;
    public float speed;
    public byte count;
    public float fireDelay;
    public float penetrate;
}
