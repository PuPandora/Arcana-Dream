using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "Scriptable Object/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float speed;
    public short health;
    public short maxHealth;
    public ExpItemData expItemData;
    public Preset collPreset;

    public RuntimeAnimatorController animController;
}
