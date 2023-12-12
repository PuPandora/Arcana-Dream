using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackText;
    public byte slotIndex;

    // Drag Drop
    Transform originalParent;
    private byte originalSiblingIndex;

    /// <summary>
    /// 슬롯 자신의 인덱스와 동일한 인벤토리 인덱스를 탐색해 UI를 업데이트합니다.
    /// </summary>
    public void ApplyInventoryData()
    {
        var slot = GameManager.instance.inventory.inventory[slotIndex];
        if (slot.isEmpty)
        {
            itemIcon.sprite = null;
            itemIcon.color = Color.clear;
            stackText.gameObject.SetActive(false);
        }
        else
        {
            itemIcon.sprite = slot.itemData.sprite;
            itemIcon.color = Color.white;
            stackText.text = slot.stack.ToString();
            stackText.gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 상점이 아니면 X
        if (GameManager.instance.playerState != PlayerState.Shop) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.instance.inventory.SellItem(slotIndex);
            if (GameManager.instance.inventory.inventory[slotIndex].itemData == null)
            {
                UIManager.instance.itemTooltip.HideUI();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 툴팁 띄우기
        if (GameManager.instance.inventory.inventory[slotIndex].itemData == null)
            return;

        UIManager.instance.itemTooltip.itemData = GameManager.instance.inventory.inventory[slotIndex].itemData;
        UIManager.instance.itemTooltip.UpdateText();
        UIManager.instance.itemTooltip.ShowUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!UIManager.instance.itemTooltip.isOpen)
            return;

        // 툴팁 닫기
        UIManager.instance.itemTooltip.HideUI();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 중");
        itemIcon.transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 각종 드래그 시작할 때의 설정
        originalParent = transform;
        originalSiblingIndex = (byte)itemIcon.transform.GetSiblingIndex();
        itemIcon.raycastTarget = false;
        UIManager.instance.inventoryUI.selectedSlot = this;
        stackText.gameObject.SetActive(false);

        itemIcon.transform.SetParent(UIManager.instance.inventoryUI.transform);
        Debug.Log(UIManager.instance.inventoryUI.transform);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 각종 드래그가 끝날 때의 설정
        itemIcon.transform.SetParent(originalParent);
        itemIcon.transform.SetSiblingIndex(originalSiblingIndex);
        itemIcon.transform.localPosition = Vector3.zero;
        itemIcon.raycastTarget = true;

        ApplyInventoryData();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 같은 슬롯의 아이템이면 생략
        if (slotIndex == UIManager.instance.inventoryUI.selectedSlot.slotIndex) return;

        var selectedSlotIndex = UIManager.instance.inventoryUI.selectedSlot.slotIndex;
        var slotA = GameManager.instance.inventory.inventory[slotIndex];
        var slotB = GameManager.instance.inventory.inventory[selectedSlotIndex];

        // 슬롯 데이터 교체
        SwapInventorySlot(slotA, slotB);

        // 데이터 교체 후 슬롯 UI 업데이트
        ApplyInventoryData();
        ApplyInventoryData();
    }

    private void SwapInventorySlot(InventorySlot slotA, InventorySlot slotB)
    {
        // 인벤토리 데이터 교환
        var tempData = slotA.itemData;
        var tempId = slotA.id;
        var tempStack = slotA.stack;
        var tempEmpty = slotA.isEmpty;

        slotA.itemData = slotB.itemData;
        slotA.id = slotB.id;
        slotA.stack = slotB.stack;
        slotA.isEmpty = slotB.isEmpty;

        slotB.itemData = tempData;
        slotB.id = tempId;
        slotB.stack = tempStack;
        slotB.isEmpty = tempEmpty;
    }
}
