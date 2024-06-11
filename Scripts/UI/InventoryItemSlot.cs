using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour
{
    [Header("Slot information")]
    public ItemSO item;
    public UIInventory inventory;

    [Header("SlotDescription")]
    public Button button;
    public Image icon;
    public Text quantityTxt;
    public int index;
    public int quantity;
    public bool equipped;

    public event Action onRefresh;

    public void Init(UIInventory inven, int i)
    {
        inventory = inven;
        index = i;
    }

    public void SetSlot()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.itemIcon;
        quantityTxt.text = quantity > 1 ? quantity.ToString() : string.Empty;
        equipped = false;
        onRefresh?.Invoke();
    }
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityTxt.text = string.Empty;
        onRefresh?.Invoke();
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
