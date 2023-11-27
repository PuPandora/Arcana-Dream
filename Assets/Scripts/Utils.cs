using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Utils : MonoBehaviour
{
    //public static WaitForFixedUpdate waitForFixedUpdate;
    public static WaitForSeconds delay0_1 = new WaitForSeconds(0.1f);
    public static WaitForSeconds delay0_25 = new WaitForSeconds(0.25f);

    public static Vector2 GetRandomVector(float min, float max)
    {
        return new Vector2(Random.Range(min, max), Random.Range(min, max));
    }

    public static Vector2 GetRandomVector(float minX, float maxX, float minY, float maxY)
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}
