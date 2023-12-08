using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStateUpgradeData", menuName = "Scriptable Object/State Upgrade Data")]
public class StateUpgradeData : ScriptableObject
{
    [PreviewField(50, ObjectFieldAlignment.Left)]
    public Sprite icon;
    public string stateName;
    public short level;
    public short maxLevel;
    public short totalIncreaseValue;
    public short totalMoreValue;
    [InfoBox("n% 스탯 증가\nState * Increase * More")]
    public StateUpgradeTable[] stateUpgradeTables;

    // 추가해야 할 스탯
    // 공격력 O
    // 체력 O
    // 체력 재생
    // 방어력 O
    // 이동 속도
    // 투사체 속도
    // 발사 속도
    // 행운
    // 시작 레벨

    public void ApplyState(short level)
    {
        // 혹시나 스탯 값이 업데이트로 변경될 때를 대비해
        // 레벨을 기준으로 다시 적용시키는 방식
        totalIncreaseValue = 0;
        totalMoreValue = 0;

        for (byte i = 0; i < level; i++)
        {
            totalIncreaseValue += stateUpgradeTables[i].increaseValue;
            totalMoreValue += stateUpgradeTables[i].moreValue;
        }
    }
}

[Serializable]
public class StateUpgradeTable
{
    public byte increaseValue;
    public byte moreValue;
    public long cost;

    [Title("State Description", bold: false)]
    [HideLabel]
    [MultiLineProperty(4)]
    public string desc;
}