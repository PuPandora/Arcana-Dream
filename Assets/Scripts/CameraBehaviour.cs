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
}
