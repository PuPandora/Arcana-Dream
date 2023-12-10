using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Transition : MonoBehaviour
{
    RectTransform rect;
    [field: SerializeField]
    public DOTweenAnimation transitionA { get; private set; }
    [field: SerializeField]
    public DOTweenAnimation transitionB { get; private set; }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        SetRectScale(Vector3.one);
    }

    public void Open()
    {
        transitionA.DORestartById("Open");
        transitionB.DORestartById("Open");
    }

    public void Close()
    {
        transitionA.DORestartById("Close");
        transitionB.DORestartById("Close");
    }

    public void CloseAndExitStage()
    {
        transitionA.DORestartById("Close");
        transitionB.DORestartById("Close");

        // 아무 트렌지션에 끝났을 때 스테이지 나가기
        var transition = transitionA.GetComponents<DOTweenAnimation>()[1];
        transition.onComplete.AddListener(GameManager.instance.ExitStage);
    }

    public void SetRectScale(Vector3 scale)
    {
        rect.localScale = scale;
    }
}
