using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotBtn : MonoBehaviour, IPointerClickHandler
{
    public GameObject quickPanel;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClickAction();
        }
        else
        {
            quickPanel.SetActive(false);
        }
    }

    void RightClickAction()
    {
        RectTransform buttonRectTransform = GetComponent<RectTransform>();
        RectTransform quickPanelRectTransform = quickPanel.GetComponent<RectTransform>();

        Vector3 buttonPosition = buttonRectTransform.position;

        quickPanelRectTransform.position = new Vector3(buttonPosition.x + 100, buttonPosition.y, buttonPosition.z);

        quickPanel.SetActive(true);
        QuickBtnPanel quickBtnPanel = quickPanel.GetComponent<QuickBtnPanel>();
        HandItemSlot handItemSlot = this.GetComponent<HandItemSlot>();
        if (handItemSlot.inventoryItemSlot.item == null)
        {
            quickPanel.SetActive(false);
            return;
        }
        quickBtnPanel.SetBtn(handItemSlot.inventoryItemSlot);
        CharacterManager.Instance.inventory.SelectItem(handItemSlot.inventoryItemSlot.index);
    }
}
