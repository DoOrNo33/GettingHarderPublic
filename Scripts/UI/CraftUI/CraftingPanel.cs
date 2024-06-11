using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingPanel : MonoBehaviour
{
    [SerializeField]
    public Dictionary<Define.CraftType, List<CraftingSO>> dicCatalog = new Dictionary<Define.CraftType, List<CraftingSO>>();
    [SerializeField]
    private List<ToggleSlot> toggleSlot;
    [SerializeField]
    private List<CreateSlot> createSlot;
    [SerializeField]
    private Define.CraftType[] type;
    // Start is called before the first frame update
    void Start()
    {
        toggleSlot = gameObject.GetComponentsInChildren<ToggleSlot>().ToList();
        createSlot = gameObject.GetComponentsInChildren<CreateSlot>().ToList();
        Init();
    }
    private void Init()
    {
        dicCatalog.Clear();
        dicCatalog = CraftManager.Instance.dicCatalog;
        CreatSlots();
    }

    private void CreatSlots()
    {
        for (int i = 0; i < toggleSlot.Count; i++)
        {
            toggleSlot[i].contentRectTr = createSlot[i].GetComponent<RectTransform>();
            toggleSlot[i].SetLabel(type[i]);
            createSlot[i].craftingSOs = dicCatalog[type[i]];
            createSlot[i].type = type[i];
            createSlot[i].gameObject.SetActive(false);
        }
        if (UIManager.Instance.CraftPanel == null)
        {
            return;
        }
        UIManager.Instance.CraftPanel.SetActive(false);
    }
}
