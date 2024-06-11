using UnityEngine;
using UnityEngine.UI;

public class HandItemSlot : MonoBehaviour
{
    [Header("Slot information")]
    public InventoryItemSlot inventoryItemSlot;
    public UIInventory inventory;

    [Header("Slot Description")]
    public Button button;
    public Image icon;
    public Text quantityTxt;

    public void init(UIInventory inven, int i)
    {
        inventory = inven;
        inventoryItemSlot = inventory.slots[i];
        inventoryItemSlot.onRefresh += HandSlotUpdate;
    }
    public void SetSlot()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = inventoryItemSlot.item.itemIcon;
        quantityTxt.text = inventoryItemSlot.quantityTxt.text;
    }
    public void Clear()
    {
        icon.gameObject.SetActive(false);
        quantityTxt.text = string.Empty;
    }

    public void HandSlotUpdate() // QuickSlotUpdate
    {
        if (inventoryItemSlot.item != null)
        {
            SetSlot();
        }
        else
        {
            Clear();
        }
    }
}
