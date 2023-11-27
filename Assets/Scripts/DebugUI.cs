using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
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

    public WeaponData weaponData;

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
                m_button.onClick.AddListener(() => AddWeapon(0));
                break;
            case UIType.Weapon1:
                m_button.onClick.AddListener(() => AddWeapon(1));
                break;
            case UIType.DeactiveWeapon:
                m_button.onClick.AddListener(ActiveWeaponButton);
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

    public void AddWeapon(int index)
    {
        var instantWeapon = Instantiate(new GameObject());
        instantWeapon.name = $"Weapon {index}";

        var weapon = instantWeapon.AddComponent<Weapon>();
        weapon.Initialize(weaponData);
        GameManager.instance.weapons[index] = weapon;

        instantWeapon.transform.parent = GameManager.instance.player.transform;
        instantWeapon.transform.localPosition = Vector3.zero;
        instantWeapon.transform.rotation = Quaternion.identity;
    }

    public void ActiveWeaponButton()
    {
        bool isDeactive = !GameManager.instance.weapons[0].gameObject.activeSelf;

        if (isDeactive)
        {
            foreach (var item in GameManager.instance.weapons)
            {
                item.gameObject.SetActive(true);
            }
            m_button.GetComponentInChildren<TextMeshProUGUI>().text = "비활성화";
        }
        else
        {
            foreach (var item in GameManager.instance.weapons)
            {
                item.gameObject.SetActive(false);
            }
            m_button.GetComponentInChildren<TextMeshProUGUI>().text = "활성화";
        }
    }
}
