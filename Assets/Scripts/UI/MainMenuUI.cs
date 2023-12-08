using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public DOTweenAnimation[] mainButtonTweens;
    public DOTweenAnimation[] selectMemoryTweens;
    public Button newGameButton;
    public Button continueButton;

    void Start()
    {
        // 임시 새 게임, 불러오기 게임 매니저 함수 AddListener
        newGameButton.onClick.AddListener(GameManager.instance.OnNewGameButton);
        continueButton.onClick.AddListener(GameManager.instance.OnContinueGameButton);
    }

    public void DoPlayMainButtonTweens(string id)
    {
        foreach (var tween in mainButtonTweens)
        {
            tween.DOPlayById(id);
        }
    }

    public void DoCompleteMainButtons()
    {
        foreach (var tween in mainButtonTweens)
        {
            tween.DOComplete();
        }
    }

    public void DoPlaySelectMemories(string id)
    {
        foreach (var tween in selectMemoryTweens)
        {
            tween.DORestartById(id);
        }
    }

    public void SelectedSaveData(int index)
    {
        for (int i = 0; i < selectMemoryTweens.Length; i++)
        {
            if (i == index) continue;

            selectMemoryTweens[i].DOPause();
            selectMemoryTweens[i].DORestartById("Hide");
        }
    }
}
