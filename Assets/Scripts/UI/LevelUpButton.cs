using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{
    private Button levelUpButton;
    private Image itemIcon;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemDescription;
    private TextMeshProUGUI itemLevel;
    private byte level = 0;

    public WeaponData weaponData;
    private PlayerWeaponController weaponController;
    private UILevelUp uiLevelUp;

    void Awake()
    {
        levelUpButton = GetComponent<Button>();
        levelUpButton.onClick.AddListener(OnClick);

        itemIcon = GetComponentsInChildren<Image>()[1];
        itemIcon.sprite = weaponData.icon;

        var texts = GetComponentsInChildren<TextMeshProUGUI>();
        itemLevel = texts[0];
        itemName = texts[1];
        itemName.text = weaponData.weaponName;
        itemDescription = texts[2];

        uiLevelUp = GetComponentInParent<UILevelUp>();
        uiLevelUp.OnShowUI += InitalizeDesc;
    }

    public void InitalizeDesc()
    {
        if (weaponController)
        {
            level = weaponController.weapon.property.level;
        }

        if (level >= weaponData.levelUpStates.Length) return;

        float damage = weaponData.levelUpStates[level].damage;
        float speed = weaponData.levelUpStates[level].speed;
        byte count = weaponData.levelUpStates[level].count;
        float fireDelay = weaponData.levelUpStates[level].fireDelay;

        itemLevel.text = $"Lv.{level}";

        itemDescription.text = string.Empty;

        if (level <= 0)
        {
            Debug.Log("아이템 레벨 0 이하");
            itemDescription.text = weaponData.desc;
            return;
        }
        Debug.Log("아이템 레벨 1 이상");
        // 임시 코드
        // 전략패턴 필요 요망
        switch (weaponData.id)
        {
            case 0: // 회전 카드 (다이아몬드 A)
                if (damage > 0) itemDescription.text += $"대미지 {damage * 100}% 증가\n";
                if (speed > 0) itemDescription.text += $"회전 속도 {speed * 100}% 증가\n";
                if (count > 0) itemDescription.text += $"회전체 {count}개 증가";
                break;
            case 1: // 던지는 카드 (클로버 A)
                if (damage > 0) itemDescription.text += $"대미지 {damage * 100}% 증가\n";
                if (speed > 0) itemDescription.text += $"투사체 속도 {speed * 100}% 증가\n";
                if (fireDelay > 0) itemDescription.text += $"공격 속도 {fireDelay * 100}% 증가";
                break;
            case 2: // 전체 대미지 증가 (하트 Q)
                itemDescription.text += $"전체 대미지 {damage * 100}% 증폭\n";
                break;
        }
    }

    public void OnClick()
    {
        // 레벨 0
        if (level == 0)
        {
            switch (weaponData.id)
            {
                case 0:
                    InstantiateWeapon<RotateCard>();
                    break;
                case 1:
                    InstantiateWeapon<ThrowCard>();
                    break;
                case 2:
                    InstantiateWeapon<HeartQueen>();
                    break;
            }

            level = weaponController.weapon.property.level;
        }
        // 레벨 1~
        else
        {
            float damage = weaponData.levelUpStates[level].damage;
            float speed = weaponData.levelUpStates[level].speed;
            float fireDelay = weaponData.levelUpStates[level].fireDelay;
            byte count = weaponData.levelUpStates[level].count;

            weaponController.weapon.LevelUp(weaponData.levelUpStates[level]);
            level = weaponController.weapon.property.level;

            if (level >= weaponData.levelUpStates.Length)
            {
                levelUpButton.interactable = false;
            }
        }
    }

    public void InstantiateWeapon<T>() where T : PlayerWeapon
    {
        GameObject newItem = new GameObject();
        newItem.name = weaponData.name;
        weaponController = newItem.AddComponent<PlayerWeaponController>();
        var instantWeapon = newItem.AddComponent<T>();
        instantWeapon.data = weaponData;
        weaponController.weapon = instantWeapon;
    }

    void OnDestroy()
    {
        uiLevelUp.OnShowUI -= InitalizeDesc;
    }
}
