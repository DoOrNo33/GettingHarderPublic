using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CategorySlot : MonoBehaviour
{
    public CraftingSO craftInfo;
    [SerializeField]
    private Image slotIcon;
    [SerializeField]
    private TextMeshProUGUI slotName;
    [SerializeField]
    private TextMeshProUGUI slotDescription;
    [SerializeField]
    private RequirementSlot slotPrefab;
    [SerializeField]
    private RequirementSlot[] slotRequirements;
    [SerializeField] 
    private GameObject slotParent;
    [SerializeField]
    private Button slotCraftBtn;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    public void CatergoryInit(CraftingSO so)
    {
        craftInfo = so;
        slotIcon.sprite = so.craftItem.itemIcon;
        slotName.text = so.craftItem.itemName;
        slotDescription.text = so.craftItem.itemDescription;
        RequirementSlotInit(so);
        slotCraftBtn.onClick.AddListener(() => CraftManager.Instance.OnClickCraft(craftInfo));
    }

    private void RequirementSlotInit(CraftingSO so)
    {
        for(int i = 0;  i < so.requirementInfo.Length; i++) 
        {
            RequirementSlot slot = Instantiate(slotPrefab, slotParent.transform);
            slot.SlotInit(so.requirementInfo[i], so.requirementCount[i]);
        }
    }
}
