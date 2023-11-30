using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Presets;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "Scriptable Object/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Title("Basic Property")]
    public float speed;
    public short maxHealth;
    public short id;
    public Preset collPreset;
    public RuntimeAnimatorController animController;

    [Title("Drop Table")]
    public ExpItemData expItemData;
    [Space(10)]
    public ItemDropTable[] itemDropTable;
}
