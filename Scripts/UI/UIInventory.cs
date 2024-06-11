using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIInventory : MonoBehaviour
{
    public InventoryItemSlot[] slots;
    public HandItemSlot[] handSlots;
    public EquipItemSlot[] equipSlot;

    [Header("Object")]
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform handSlotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemNameDescription;
    public TextMeshProUGUI selectedItemStartName;
    public TextMeshProUGUI selectedItemStartValue;
    public GameObject useBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject dropBtn;

    private PlayerController controller;
    private PlayerCondition condition;

    public ItemSO selectedItem;
    public int selectedItemIndex = 0;

    int equipIndex = 0; 

    int curEquipIndex;
    
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        CharacterManager.Instance.inventory = this;

        controller.inventory += Toggle;

        inventoryWindow.SetActive(false);
        slots = new InventoryItemSlot[slotPanel.childCount];

        CharacterManager.Instance.Player.addItem += AddItem;

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<InventoryItemSlot>();
            slots[i].Init(this, i);
        }

        handSlots = new HandItemSlot[handSlotPanel.childCount];
        for (int i = 0; i < handSlots.Length; i++)
        {
            handSlots[i] = handSlotPanel.GetChild(i).GetComponent<HandItemSlot>();
            handSlots[i].init(this, i);
        }
        ClearSelcetedItemWindow();
        UpdateUI();
    }
    void ClearSelcetedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemNameDescription.text = string.Empty;
        selectedItemStartName.text = string.Empty;
        selectedItemStartValue.text = string.Empty;

        useBtn.gameObject.SetActive(false);
        equipBtn.gameObject.SetActive(false);
        unEquipBtn.gameObject.SetActive(false);
        dropBtn.gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool isOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemSO data = CharacterManager.Instance.Player.itemSO;
        if (data.canStack)
        {
            InventoryItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemSO = null;
                return;
            }
        }

        InventoryItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemSO = null;
            return;
        }
    }

    public void AddItem(ItemSO data, int count)
    {
        if (data.canStack)
        {
            InventoryItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity += count;
                UpdateUI();
                return;
            }
        }

        InventoryItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity += count;
            UpdateUI();
            return;
        }
    }

    void ThrowItem(ItemSO data)
    {
        Instantiate(data.itemPrefab, dropPosition.position, Quaternion.Euler(0,Random.value * 360,0));
    }

    public void UpdateUI()
    {
        for(int i = 0; i<slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].SetSlot();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    private InventoryItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    private InventoryItemSlot GetItemStack(ItemSO data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        useBtn.SetActive(false);
        equipBtn.SetActive(false);
        unEquipBtn.SetActive(false);
        if (slots[index].item == null)
        {
            ClearSelcetedItemWindow();
            return;
        }
        

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.itemName;
        selectedItemNameDescription.text = selectedItem.itemDescription;

        selectedItemStartName.text = string.Empty;
        selectedItemStartValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedItemStartName.text += selectedItem.consumables[i].consumableType.ToString() + "\n";
            selectedItemStartValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        switch (selectedItem.itemType)
        {
            case Define.CraftType.Food:
                useBtn.SetActive(true);
                break;
            case Define.CraftType.Weapon:
                SetActiveEquipBtn(index);
                break;
            case Define.CraftType.Tool:
                SetActiveEquipBtn(index);
                break;
            case Define.CraftType.Wearable:
                SetActiveEquipBtn(index);
                break;
            default:
                break;
        }
        dropBtn.SetActive(true);
    }

    void SetActiveEquipBtn(int index)
    {
        if (!slots[index].equipped)
        {
            equipBtn.SetActive(true);
        }
        else
        {
            unEquipBtn.SetActive(true);
        }
    }

    public void OnUseBtn()
    {
        if(selectedItem.itemType == Define.CraftType.Food)
        {
            for(int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch(selectedItem.consumables[i].consumableType)
                {
                    case Define.ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case Define.ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }
    public void OnDropBtn()
    {
        if (slots[selectedItemIndex] && slots[selectedItemIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelcetedItemWindow();
        }
        UpdateUI();
    }

    public void RemoveSelectedItem(int index, int count)
    {
        slots[index].quantity -= count;

        if (slots[index].quantity <= 0)
        {
            selectedItem = null;
            slots[index].item = null;
            ClearSelcetedItemWindow();
        }
        UpdateUI();
    }
    
    public void OnEquipBtn()
    {
        SetEquipIndex();
        if (slots[curEquipIndex].equipped)
        {
            SetEquipIndex();
            UnEquip(equipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = equipIndex;
        switch (selectedItem.itemType)
        {
            case Define.CraftType.Weapon:
                equipSlot[0].MoveData(slots[selectedItemIndex]);
                CharacterManager.Instance.Player.equipment.EquipNew(selectedItem, 0);
                RemoveSelectedItem();
                break;
            case Define.CraftType.Tool:
                equipSlot[0].MoveData(slots[selectedItemIndex]);
                CharacterManager.Instance.Player.equipment.EquipNew(selectedItem, 0);
                RemoveSelectedItem();
                break;
            case Define.CraftType.Wearable:
                SetEquipIndex();
                equipSlot[equipIndex].MoveData(slots[selectedItemIndex]);
                CharacterManager.Instance.Player.equipment.EquipNew(selectedItem, equipIndex);
                RemoveSelectedItem();
                break;

        }

    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        SetEquipIndex();
        CharacterManager.Instance.Player.equipment.UnEquip(equipIndex);
        UpdateUI();

        if(selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }
    public void OnUnEquipBtn()
    {
        UnEquip(selectedItemIndex);
    }

    public void ChangeItemSlot(InventoryItemSlot target1, InventoryItemSlot target2) // 아이템 슬롯 1과 2의 정보를 받아옴
    {
        ItemSO tempItem = target2.item;             //target2에 대한 정보를 미리 저장
        int tempQuantity = target2.quantity;
        bool tempEquipped = target2.equipped;

        target2.quantity = target1.quantity;        //target1과 2의 정보를 교환
        target2.equipped = target1.equipped;
        target2.item = target1.item;

        target1.item = tempItem;
        target1.quantity = tempQuantity;
        target1.equipped = tempEquipped;

        if (target1.item != null)
        {
            target1.SetSlot();
        }
        else
        {
            target1.Clear();
        }

        if (target2.item != null)
        {
            target2.SetSlot();
        }
        else
        {
            target2.Clear();
        }
    }
    void SetEquipIndex()
    {
        if (selectedItem.itemID.Equals(1001)) equipIndex = 1;
        else if (selectedItem.itemID.Equals(1002)) equipIndex = 2;
        else if (selectedItem.itemID.Equals(1003)) equipIndex = 3;
        else equipIndex = 0;
    }

    public void AddItem(ItemSO data, int count, bool equiped)
    {
        if (data.canStack)
        {
            InventoryItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity += count;
                UpdateUI();
                return;
            }
        }

        InventoryItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity += count;
            UpdateUI();
            return;
        }
    }
}
