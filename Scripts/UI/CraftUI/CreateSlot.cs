using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    public List<CraftingSO> craftingSOs;
    public Define.CraftType type;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        
    }

    public void Init()
    {
        craftingSOs = CraftManager.Instance.dicCatalog[type];
        for (int i = 0; i < craftingSOs.Count; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);
            slot.GetComponent<CategorySlot>().CatergoryInit(craftingSOs[i]);
        }
    }
}
