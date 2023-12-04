using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : LobbyUI
{
    private InventorySlotUI[] slots;
    private Inventory inventory;
    [SerializeField] private TextMeshProUGUI goldText;
    public InventorySlotUI selectedSlot;

    protected override void Awake()
    {
        base.Awake();

        slots = GetComponentsInChildren<InventorySlotUI>();
        GameManager.instance.OnGoldChanged += UpdateGoldText;

        for (byte i = 0; i < slots.Length; i++)
        {
            slots[i].slotIndex = i;
        }

        UpdateGoldText();
    }

    protected override void Start()
    {
        inventory = GameManager.instance.inventory;
        base.Start();
    }

    protected override void Initialize()
    {
        UIManager.instance.inventoryUI = this;
        HideUI();
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
            slots[i].ApplyInventoryData();
        }
    }

    /// <summary>
    /// 특정 인덱스의 인벤토리 슬롯을 업데이트합니다.
    /// </summary>
    /// <param name="index"></param>
    public void UpdateInventory(byte index)
    {
        slots[index].ApplyInventoryData();
    }

    public override void ShowUI()
    {
        UpdateGoldText();
        UpdateInventory();

        base.ShowUI();
    }

    public override void HideUI()
    {
        if (UIManager.instance.itemTooltip)
        {
            UIManager.instance.itemTooltip.HideUI();
        }
        base.HideUI();
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
