using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    void Awake()
    {
        GameManager.instance.vCam = GetComponent<CinemachineVirtualCamera>();
    }

    void FixedUpdate()
    {
        Debug.Log("카메라 위치 재적용");
        var targetPos = GameManager.instance.player.transform.position;
        transform.position = new Vector3(targetPos.x, targetPos.y, -10);
    }
}
