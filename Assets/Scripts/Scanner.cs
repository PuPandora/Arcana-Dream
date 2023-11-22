using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;

    private RaycastHit2D[] targets;
    [ReadOnly]
    public Transform nearestTarget;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position,
                                scanRange,
                                Vector2.zero,
                                0,
                                targetLayer);

        GetNearestTarget();
    }

    private void GetNearestTarget()
    {
        float diff = 100f;
        Vector2 myPos = transform.position;

        foreach (var target in targets)
        {
            Vector2 targetPos = target.transform.position;           
            float curDiff = Vector2.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                nearestTarget = target.transform;
            }
        }
    }
}
