using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackText;

    public void ApplyInventoryData(InventorySlot slot)
    {
        if (slot.itemData == null)
        {
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false);
            stackText.gameObject.SetActive(false);
            return;
        }

        itemIcon.sprite = slot.itemData.sprite;
        itemIcon.gameObject.SetActive(true);
        stackText.text = slot.stack.ToString();
        stackText.gameObject.SetActive(true);
    }
}
