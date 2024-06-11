using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CraftManager : Singleton<CraftManager>
{
    public Dictionary<Define.CraftType, List<CraftingSO>> dicCatalog = new Dictionary<Define.CraftType, List<CraftingSO>>();
    [SerializeField]
    private List<CraftingSO> toolCatalog;
    [SerializeField]
    private List<CraftingSO> weaponCatalog;
    [SerializeField]
    private List<CraftingSO> foodCatalog;
    [SerializeField]
    private List<CraftingSO> buildingCatalog;
    [SerializeField]
    private List<CraftingSO> propCatalog;
    [SerializeField]
    private List<CraftingSO> wearableCatalog;
    [SerializeField]
    private List<CraftingSO> resourceCatalog;

    public bool isBuilding = false;
    public bool canBuilding = false;
    public GameObject selectedBluePrint;
    [SerializeField]
    private CraftingSO selectedCraftItem;
    private Vector3 movePos = Vector3.zero;
    [SerializeField]
    private LayerMask layermask;
    [SerializeField]
    private GameObject locate;
    [SerializeField]
    private List<InventoryItemSlot> slots;
    [SerializeField]
    private List<int> slotIndex = new List<int>();

    [Header("제작 실패UI")]
    [SerializeField]
    private TextMeshProUGUI failedTxt;
    [SerializeField]
    private Coroutine onText;
    [SerializeField]
    private float onTextTime;

    [Header("효과음")]
    [SerializeField]
    private AudioSource audioSource;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        dicCatalog[Define.CraftType.Tool] = toolCatalog;
        dicCatalog[Define.CraftType.Weapon] = weaponCatalog;
        dicCatalog[Define.CraftType.Food] = foodCatalog;
        dicCatalog[Define.CraftType.Building] = buildingCatalog;
        dicCatalog[Define.CraftType.Prop] = propCatalog;
        dicCatalog[Define.CraftType.Wearable] = wearableCatalog;
        dicCatalog[Define.CraftType.Resource] = resourceCatalog;
    }

    private void LateUpdate()
    {
        if(selectedBluePrint != null)
            selectedBluePrint.transform.position = Vector3.Lerp(selectedBluePrint.transform.position, movePos, 0.5f);
    }

    public void Build()
    {
        if (canBuilding)
        {
            isBuilding = false;
            RemoveItem(selectedCraftItem);
            GameObject go = Instantiate(selectedCraftItem.craftItem.itemPrefab, selectedBluePrint.transform.position, selectedBluePrint.transform.rotation);
            go.transform.position = selectedBluePrint.transform.position;
            audioSource.PlayOneShot(selectedCraftItem.craft_sound);
            Destroy(selectedBluePrint);

            selectedBluePrint = null;
        }
    }

    public void CanceledBuild()
    {
        isBuilding = false;
        Destroy(selectedBluePrint);
        selectedBluePrint = null;
    }

    //건설
    public void OnBuilding(InputAction.CallbackContext context)
    {
        if (!isBuilding) return;

        Vector3 mousePoint = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 point = hit.point;
            float sizeY = 0; ;
            if (selectedBluePrint.TryGetComponent<BoxCollider>(out var collBox))
            {
                sizeY = collBox.bounds.center.y - collBox.size.y * 0.5f; ;
            }
            movePos = Vector3.Lerp(selectedBluePrint.transform.position, new Vector3(point.x, sizeY, point.z), 0.5f);
            if (selectedBluePrint.GetComponent<BluePrint>().list.Count == 0)
            {
                canBuilding = true;
            }
        }
    }

    IEnumerator MoveToPoint()
    {
        yield return null;
    }

    public void OnClickCraft(CraftingSO so)
    {
        if (isBuilding) return;
        selectedCraftItem = so;
        if (!CheckRequirmentItem(selectedCraftItem)) 
        {
            if(onText != null)
                StopCoroutine(onText);
            failedTxt.gameObject.SetActive(true);
            onText = StartCoroutine(OnFailedText());
            return;
        }

        
        if (selectedCraftItem.craftItem.itemType != Define.CraftType.Building)
        {
            
            UIManager.Instance.CraftPanel.SetActive(false);
            RemoveItem(so);
            audioSource.PlayOneShot(so.craft_sound);
            CharacterManager.Instance.inventory.AddItem(selectedCraftItem.craftItem, 1);
        }
        else
        {
            UIManager.Instance.ConstructurePanel.SetActive(false);
            GameObject go = Instantiate(selectedCraftItem.bluePrint);
            selectedBluePrint = go;
            isBuilding = true;
            StartCoroutine(MoveToPoint());
        }
    }

    public bool CheckRequirmentItem(CraftingSO so)
    {      
        GetPlayerInventroyItem();
        bool[] isClear = new bool[so.requirementInfo.Length];
        for (int i = 0; i < so.requirementInfo.Length; i++)
        {
            foreach (InventoryItemSlot slot in slots)
            {
                if (slot.item != null && slot.item.itemID == so.requirementInfo[i].itemID && slot.quantity >= so.requirementCount[i])
                {
                    isClear[i] = true;
                    slotIndex.Add(slot.index);
                    break;
                }
            }
            if (!isClear[i]) return false;
        }
        return true;
    }

    public void GetPlayerInventroyItem()
    {
        slots = CharacterManager.Instance.inventory.slots.ToList();
    }

    public void CheckedCanBuilding(GameObject go, bool result)
    {
        if(go == selectedBluePrint)
            canBuilding = result;
    }

    public void RemoveItem(CraftingSO so)
    {
        for (int i = 0; i < so.requirementInfo.Length; i++)
        {
            CharacterManager.Instance.inventory.RemoveSelectedItem(slotIndex[i], so.requirementCount[i]);
        }
        slotIndex.Clear();
    }

    IEnumerator OnFailedText()
    {
        yield return new WaitForSeconds(onTextTime);
        failedTxt.gameObject.SetActive(false);
        onText = null;
    }
}

