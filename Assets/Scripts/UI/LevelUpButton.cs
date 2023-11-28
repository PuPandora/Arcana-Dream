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
    private byte level;

    public WeaponData weaponData;
    private Weapon weapon;

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
    }

    void OnEnable()
    {
        if (level >= weaponData.levelState.Length)
            return;

        float damage = weaponData.levelState[level].damage;
        float speed = weaponData.levelState[level].speed;
        byte count = weaponData.levelState[level].count;
        float fireDelay = weaponData.levelState[level].fireDelay;

        itemLevel.text = $"Lv.{level}";

        itemDescription.text = string.Empty;

        if (level <= 0)
        {
            itemDescription.text = weaponData.desc;
            return;
        }
        // 임시 코드
        // 전략패턴 필요 요망
        switch (weaponData.id)
        {
            case 0: // 회전 카드 (다이아몬드 A)
                itemDescription.text += $"대미지 {damage * 100 - 100}% 증가\n";
                itemDescription.text += $"회전 속도 {speed * 100 - 100}% 증가\n";
                itemDescription.text += $"회전체 {count}개 증가";
                break;
            case 1: // 던지는 카드 (클로버 A)
                itemDescription.text += $"대미지 {damage * 100 - 100}% 증가\n";
                itemDescription.text += $"투사체 속도 {speed * 100 - 100}% 증가\n";
                itemDescription.text += $"공격 속도 {fireDelay * 100 - 100}% 증가";
                break;
            case 2: // 전체 대미지 증가 (하트 Q)
                itemDescription.text += $"전체 대미지 {damage * 100 - 100}% 증폭\n";
                break;
        }
    }

    public void OnClick()
    {
        // 레벨 0
        if (level == 0)
        {
            if (weaponData.id == 0 || weaponData.id == 1)
            {
                GameObject newItem = new GameObject();
                newItem.name = weaponData.name;
                weapon = newItem.AddComponent<Weapon>();
                weapon.Initialize(weaponData);
            }
            else if (weaponData.id == 2)
            {
                foreach (var weapon in GameManager.instance.player.weapons)
                {
                    weapon.moreDamage += weaponData.levelState[level].damage - 1;
                    weapon.LevelUp();
                }
            }

            level++;
            if (weaponData.id == 2) return;
            GameManager.instance.player.weapons.Add(weapon);
        }
        // 레벨 1~
        else
        {
            float damage = weaponData.levelState[level].damage;
            float speed = weaponData.levelState[level].speed;
            float fireDelay = weaponData.levelState[level].fireDelay;
            byte count = weaponData.levelState[level].count;

            switch (weaponData.id)
            {
                case 0: // 임시 회전 카드
                        weapon.increaseDamage += damage - 1;
                        weapon.increaseDamage += speed - 1;
                        weapon.AddMeleeWeapon(weapon.count + count);
                    break;
                case 1: // 임시 클로머 A (원거리 공격)
                    weapon.increaseDamage += damage - 1;
                    weapon.increaseSpeed += speed - 1;
                    weapon.increaseFireDelay += fireDelay - 1;
                    break;
                case 2: // 임시 하트퀸 (전체 대미지 증가 아이템)
                    foreach (var weapon in GameManager.instance.player.weapons)
                    {
                        weapon.moreDamage += damage - 1;
                        weapon.LevelUp();
                    }
                    break;
            }            

            if (weaponData.id == 0 || weaponData.id == 1)
            {
                weapon.LevelUp();
            }
            level++;

            if (level >= weaponData.levelState.Length)
            {
                levelUpButton.interactable = false;
            }
        }
    }
}
