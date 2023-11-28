using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitStageDebugButton : MonoBehaviour
{
    private UnityEngine.UI.Button exitButton;

    void Awake()
    {
        exitButton = GetComponent<UnityEngine.UI.Button>();
    }

    void Start()
    {
        exitButton.onClick.AddListener(GameManager.instance.ExitStage);
    }
}
