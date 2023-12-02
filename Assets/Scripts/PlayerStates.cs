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

    public byte damageLevel;
    public byte healthLevel;
    public byte defenseLevel;

    private float baseHealth = 100f;

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
}
