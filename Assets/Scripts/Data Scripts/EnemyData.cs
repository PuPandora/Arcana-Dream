using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "Scriptable Object/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float speed;
    public short health;
    public short maxHealth;
    public ExpItemData expItemData;

    public RuntimeAnimatorController animController;
}
