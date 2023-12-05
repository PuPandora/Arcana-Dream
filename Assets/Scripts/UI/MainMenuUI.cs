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
