using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitStageDebugButton : MonoBehaviour
{
    private Button exitButton;

    void Awake()
    {
        exitButton = GetComponent<Button>();
    }

    void Start()
    {
        exitButton.onClick.AddListener(GameManager.instance.ExitStage);
    }
}
