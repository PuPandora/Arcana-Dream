using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnToggleDebugUI : MonoBehaviour
{
    Toggle spawnToggle;

    void Awake()
    {
        spawnToggle = GetComponent<Toggle>();
        spawnToggle.onValueChanged.AddListener((value) => { ApplySpawnToggle(value); });
    }

    // 스폰 토글 디버그 (임시 코드)
    public void ApplySpawnToggle(bool value)
    {
        StageManager.instance.spawner.isPlaying = value;
    }
}
