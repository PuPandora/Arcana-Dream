using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPortal : MonoBehaviour
{
    public string stageSceneName = "Stage";
    private bool isPlayerInPortal;
    public GameObject stageNoticeUI;
    private RectTransform noticeUiRect;

    void Awake()
    {
        noticeUiRect = stageNoticeUI.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isPlayerInPortal && Input.GetKeyDown(GameManager.instance.interactKey))
        {
            GameManager.instance.EnterStage("Stage");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInPortal = true;
            noticeUiRect.localPosition = Vector3.zero;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInPortal = false;
            if (noticeUiRect != null)
            {
                noticeUiRect.localPosition = Vector3.up * 1000;
            }
        }
    }
}
