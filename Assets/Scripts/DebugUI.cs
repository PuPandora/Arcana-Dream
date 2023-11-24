using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public enum UIType { None, Weapon0, Weapon1, DeactiveWeapon, SpawnToggle, Exit }
    [EnumPaging]
    [SerializeField]
    private UIType type;

    private Button m_button;
    private Toggle m_toggle;

    void Awake()
    {
        m_button = GetComponent<Button>();
        m_toggle = GetComponent<Toggle>();
    }

    void Start()
    {
        switch (type)
        {
            case UIType.None:
                Debug.LogWarning("초기화 되지 않은 Type.", gameObject);
                break;
            case UIType.Weapon0:
                GameManager.instance.debugWeaponBtns[0] = m_button;
                m_button.onClick.AddListener(() => GameManager.instance.ActiveWeapon(0));
                break;
            case UIType.Weapon1:
                GameManager.instance.debugWeaponBtns[1] = m_button;
                m_button.onClick.AddListener(() => GameManager.instance.ActiveWeapon(1));
                break;
            case UIType.DeactiveWeapon:
                GameManager.instance.debugWeaponBtns[2] = m_button;
                m_button.onClick.AddListener(GameManager.instance.DeactiveAllWeapons);
                break;
            case UIType.SpawnToggle:
                GameManager.instance.spawnToggle = m_toggle;
                break;
            case UIType.Exit:
                m_button.onClick.AddListener(GameManager.instance.ExitStage);
                break;
            default:
                Debug.LogError($"할당되지 않은 UIType - {type}", gameObject);
                break;
        }
    }
}
