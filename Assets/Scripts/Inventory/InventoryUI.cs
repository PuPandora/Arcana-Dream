using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private RectTransform rect;
    private InventorySlotUI[] slots;
    private Inventory inventory;
    [SerializeField] private TextMeshProUGUI goldText;
    public bool isUIOpen { get; private set; }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        slots = GetComponentsInChildren<InventorySlotUI>();
        GameManager.instance.inventoryUI = this;
        isUIOpen = false;
        GameManager.instance.OnGoldChanged += UpdateGoldText;

        for (byte i = 0; i < slots.Length; i++)
        {
            slots[i].slotIndex = i;
        }

        UpdateGoldText();
    }

    void Start()
    {
        inventory = GameManager.instance.inventory;
    }

    /// <summary>
    /// 인벤토리의 모든 슬롯을 업데이트합니다.
    /// </summary>
    [ContextMenu("Update Inventory")]
    public void UpdateInventory()
    {
        Debug.Log("Udate Inventory", gameObject);
        for (byte i = 0; i < slots.Length; i++)
        {
            slots[i].ApplyInventoryData(inventory.inventory[i]);
        }
    }

    /// <summary>
    /// 특정 위치의 인벤토리 슬롯을 업데이트합니다.
    /// </summary>
    /// <param name="index"></param>
    public void UpdateInventory(byte index)
    {
        slots[index].ApplyInventoryData(inventory.inventory[index]);
    }

    public void ShowUI()
    {
        UpdateInventory();
        rect.localScale = Vector3.one;
        isUIOpen = true;
    }

    public void HideUI()
    {
        rect.localScale = Vector3.zero;
        isUIOpen = false;
    }

    private void UpdateGoldText()
    {
        goldText.text = $"GOLD : {GameManager.instance.gold.ToString():#,###}";
    }

    void OnDestroy()
    {
        GameManager.instance.OnGoldChanged -= UpdateGoldText;
    }
}
