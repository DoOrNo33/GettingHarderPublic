using UnityEngine;

public class QuickBtnPanel : MonoBehaviour
{
    public GameObject useBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject dropBtn;

    public void SetBtn(InventoryItemSlot inventoryItemSlot)
    {
        TurnOffBtn();
        switch (inventoryItemSlot.item.itemType)
        {
            case Define.CraftType.Food:
                useBtn.SetActive(true);
                break;
            case Define.CraftType.Weapon:
                SetActiveEquipBtn(inventoryItemSlot);
                break;
            case Define.CraftType.Tool:
                SetActiveEquipBtn(inventoryItemSlot);
                break;
            case Define.CraftType.Wearable:
                SetActiveEquipBtn(inventoryItemSlot);
                break;
            default:
                break;
        }
        dropBtn.SetActive(true);
    }

    void SetActiveEquipBtn(InventoryItemSlot inventoryItemSlot)
    {
        if (!inventoryItemSlot.equipped)
        {
            equipBtn.SetActive(true);
        }
        else
        {
            unEquipBtn.SetActive(true);
        }
    }

    void TurnOffBtn()
    {
        useBtn.SetActive(false);
        equipBtn.SetActive(false);
        unEquipBtn.SetActive(false);
        dropBtn.SetActive(false);
    }

    public void UseBtn()
    {
        CharacterManager.Instance.inventory.OnUseBtn();
        this.gameObject.SetActive(false);
    }
    public void EquipBtn()
    {
        CharacterManager.Instance.inventory.OnEquipBtn();
        this.gameObject.SetActive(false);
    }
    public void UnEquipBtn()
    {
        CharacterManager.Instance.inventory.OnUnEquipBtn();
        this.gameObject.SetActive(false);
    }
    public void DropBtn()
    {
        CharacterManager.Instance.inventory.OnDropBtn();
        this.gameObject.SetActive(false);
    }
}
