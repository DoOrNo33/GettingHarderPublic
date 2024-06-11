using UnityEngine;
using UnityEngine.UI;

public class EquipItemSlot : MonoBehaviour
{
    [Header("Slot information")]
    public ItemSO item;
    public UIInventory inventory;

    [Header("Slot Description")]
    public Button button;
    public Image icon;
    public GameObject iconType;
    public Text quantityTxt;
    public int index;
    public bool equipped;
    public int quantity;

    public GameObject unEquipmentBtn;
    public Button unEquipBtn;

    int equipIndex = 0;

    private void Start()
    {
        
    }
    public void MoveData(InventoryItemSlot inventoryItemSlotData)
    {
        if (item != null)
        {
            UnEquipmentBtn();
        }
        item = inventoryItemSlotData.item;
        quantity = inventoryItemSlotData.quantity;
        equipped = inventoryItemSlotData.equipped;
        iconType.SetActive(false);
        UpdateSlot();
    }

    public void OnClickBtn()
    {
        if (item == null)
        {
            return;
        }
        else
        {
            CharacterManager.Instance.inventory.selectedItemName.text = item.itemName;
            CharacterManager.Instance.inventory.selectedItemNameDescription.text = item.itemDescription;
            CharacterManager.Instance.inventory.selectedItemStartName.text = string.Empty;
            CharacterManager.Instance.inventory.selectedItemStartValue.text = string.Empty;

            for (int i = 0; i < item.consumables.Length; i++)
            {
                CharacterManager.Instance.inventory.selectedItemName.text += item.consumables[i].consumableType.ToString() + "\n";
                CharacterManager.Instance.inventory.selectedItemStartValue.text += item.consumables[i].value.ToString() + "\n";
            }
            if (item != null) unEquipmentBtn.SetActive(true);
            unEquipBtn.onClick.RemoveAllListeners();
            unEquipBtn.onClick.AddListener(UnEquipmentBtn);
        }
        
    }
    public void UnEquipmentBtn()
    {
        SetEquipIndex();
        CharacterManager.Instance.Player.itemSO = item;
        CharacterManager.Instance.Player.equipment.UnEquip(equipIndex);
        CharacterManager.Instance.Player.addItem?.Invoke();
        item = null;
        unEquipmentBtn.SetActive(false);
        UpdateSlot();
    }

    void UpdateSlot()
    {
        if (item != null)
        {
            SetSlot();
        }
        else
        {
            Clear();
        }
    }
    void SetSlot()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.itemIcon;
        quantityTxt.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }
    void Clear()
    {
        item = null;
        iconType.SetActive(true);
        icon.gameObject.SetActive(false);
        quantityTxt.text = string.Empty;
    }
    void SetEquipIndex()
    {
        if (item.itemID.Equals(1001)) equipIndex = 1;
        else if (item.itemID.Equals(1002)) equipIndex = 2;
        else if (item.itemID.Equals(1003)) equipIndex = 3;
        else equipIndex = 0;
    }
}
