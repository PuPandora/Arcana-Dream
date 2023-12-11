using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerStates
{
    public StateUpgradeData damageState;
    public StateUpgradeData healthState;
    public StateUpgradeData defenseState;

    public short damageLevel = 0;
    public short healthLevel = 0;
    public short defenseLevel = 0;

    private float baseHealth = 100f;

    public short averageLevel
    {
        get
        {
            // 스탯이 많아질 경우는 어떻게 개선할까?
            short[] allStates = new short[] { damageLevel, healthLevel, defenseLevel };

            float result = 0f;
            foreach (short state in allStates)
            {
                result += state;
            }
            result /= allStates.Length;
            result = MathF.Round(result);

            return (short)result;
        }
    }

    public float defense
    {
        get
        {
            return (defenseState.totalIncreaseValue * (defenseState.totalMoreValue * 0.01f) + 1) * Time.deltaTime;
        }
    }

    public float health
    {
        get
        {
            return baseHealth * (healthState.totalIncreaseValue * 0.01f + 1) * (healthState.totalMoreValue * 0.01f + 1);
        }
    }

    public void Init()
    {
        damageState.level = damageLevel;
        healthState.level = healthLevel;
        defenseState.level = defenseLevel;
    }
}
