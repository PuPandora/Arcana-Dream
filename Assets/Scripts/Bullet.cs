using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType { Melee, Range }
    public int damage;
    public int per;
    public float speed;
}
