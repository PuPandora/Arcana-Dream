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

    public byte count;
    public float fireDelay;

    [Title("Bullet Property")]
    // Bullet Property
    public float damage;
    public float speed;
    public sbyte penetrate;
    public GameObject bulletPrefab;
}
