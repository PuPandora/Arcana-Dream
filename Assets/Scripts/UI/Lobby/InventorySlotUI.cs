using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackText;
    public byte slotIndex;

    public void ApplyInventoryData(InventorySlot slot)
    {
        if (slot.isEmpty)
        {
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false);
            stackText.gameObject.SetActive(false);
            return;
        }
        else
        {
            itemIcon.sprite = slot.itemData.sprite;
            itemIcon.gameObject.SetActive(true);
            stackText.text = slot.stack.ToString();
            stackText.gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.instance.inventory.SellItem(slotIndex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 툴팁 띄우기
        Debug.Log("포인터 들어옴");
        UIManager.instance.itemTooltip.itemData = GameManager.instance.inventory.inventory[slotIndex].itemData;
        UIManager.instance.itemTooltip.UpdateText();
        UIManager.instance.itemTooltip.ShowUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 툴팁 닫기
        UIManager.instance.itemTooltip.HideUI();
    }
}
