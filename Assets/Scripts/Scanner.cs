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

        // 필요할 때만 주변 적을 감지하도록 수정 예정
        // + Weapon이 가까운 적을 필요 할 때 탐색
        nearestTarget = GetNearestTarget();

        // 만약 멀티샷 무기라면, n번째로 가까운 적의 위치도 반환할 메소드
        // ㄴ 거리에 따라 정렬 필요
    }

    private Transform GetNearestTarget()
    {
        float diff = 100f;
        Transform result = null;
        Vector2 myPos = transform.position;

        foreach (var target in targets)
        {
            Vector2 targetPos = target.transform.position;           
            float curDiff = Vector2.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}
