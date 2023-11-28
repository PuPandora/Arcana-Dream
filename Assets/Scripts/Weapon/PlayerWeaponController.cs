using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerWeapon weapon;

    void Start()
    {
        weapon.InitSetting();
    }

    void Update()
    {
        weapon.Using();
    }
}
