using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerStates
{
    public PlayerState damageState = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };
    public PlayerState SpeedState = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };
    public PlayerState speedState = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };
    public PlayerState healthState = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };
    public PlayerState healthRegenState = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };
    public PlayerState defenseState = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };
    public PlayerState moveSpeedState = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };
    public PlayerState luckState  = new PlayerState { level = 0, increaseAmount = 1f, moreAmount = 1f };

    [Title("Start Level")]
    public byte startLevel = 0;

    [Serializable]
    public struct PlayerState
    {
        public byte level;
        public float increaseAmount;
        public float moreAmount;
    }
}
