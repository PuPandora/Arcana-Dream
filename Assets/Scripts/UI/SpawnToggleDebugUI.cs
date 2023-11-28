using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnToggleDebugUI : MonoBehaviour
{
    Toggle m_toggle;

    void Awake()
    {
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener((t) => { A(); });
    }

    // 스폰 토글 디버그 (임시 코드)
    public void A()
    {
        GameManager.instance.spawner.isPlaying = m_toggle.isOn;
    }
}
