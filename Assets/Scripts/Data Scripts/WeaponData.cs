using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponName", menuName = "Scriptable Object/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [EnumToggleButtons]
    public Weapon.WeaponType type;

    [Title("Weapon Property")]
    // Weapon Object Property
    public string weaponName;
    [TextArea] public string desc;
    [PreviewField] public Sprite icon;
    public byte id;

    public byte baseCount;
    public float baseFireDelay;

    [Title("Bullet Property")]
    // Bullet Property
    public float baseDamage;
    public float baseSpeed;
    public sbyte basepenetrate;
    public GameObject bulletPrefab;

    [Title("Level")]
    public levelState[] levelState;
}

[System.Serializable]
public struct levelState
{
    public float damage;
    public float speed;
    public byte count;
    public float fireDelay;
    public float penetrate;
}
