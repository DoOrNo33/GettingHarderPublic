using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RequirementSlot : MonoBehaviour
{
    [SerializeField]
    private Image slotIcon;
    [SerializeField]
    private TextMeshProUGUI slotName;
    [SerializeField]
    private TextMeshProUGUI requirementCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SlotInit(ItemSO so, int count)
    {
        slotIcon.sprite = so.itemIcon;
        slotName.text = so.itemName;
        requirementCount.text = count.ToString();
    }
}
