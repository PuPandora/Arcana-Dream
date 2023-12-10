using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    void Awake()
    {
        GameManager.instance.playerCam = GetComponent<CinemachineVirtualCamera>();
    }

    //void FixedUpdate()
    //{
    //    var targetPos = GameManager.instance.player.transform.position;
    //    transform.position = new Vector3(targetPos.x, targetPos.y, -10);
    //}
}
