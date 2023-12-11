using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "Scriptable Object/Enemy Data")]
public class EnemyData : GameObjectData
{
    [Title("Basic Property")]
    public float speed;
    public float damage;
    public short maxHealth;
    public CapsuleDirection2D collDirection;
    public Vector2 collOffset;
    public Vector2 collSize;
    public RuntimeAnimatorController animController;

    [Title("Drop Table")]
    public ExpItemData expItemData;
    [Space(10)]
    public ItemDropTable[] itemDropTable;
}
