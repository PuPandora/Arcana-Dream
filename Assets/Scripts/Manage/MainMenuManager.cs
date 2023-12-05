using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public OptionUI optionUI;
    public MainMenuUI menuUI;

    void Start()
    {
        AudioManager.instance.PlayBgm(AudioManager.Bgm.Main, true);
        Debug.Log("BGM 실행");
    }
}
