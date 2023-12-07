using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowUI : MonoBehaviour
{
    RectTransform rect;
    Camera mainCam;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        mainCam = Camera.main;
    }

    void FixedUpdate()
    {
        rect.position = GameManager.instance.player.transform.position;
    }
}
