using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;        // UI가 소속되어 있는 최상단의 Canvas
    private Transform previosParent; // 드래그 직전의 부모 오브젝트
    private RectTransform rect;      // UI 위치 제어를 위한 RectTransform

    public InventoryItemSlot inventoryitemSlot;
    public HandItemSlot handItemSlot;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 직전에 소속되어 있던 부모 Transform 정보 저장
        previosParent = transform.parent;

        // 현재 드래그중인 UI가 화면의 최상단에 출력되도록 하기 위해
        transform.SetParent(canvas);      // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling();     // 가장 앞에 보이도록 마지막 자식으로 설정

    }
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }
    public GraphicRaycaster raycaster;
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == canvas)
        {
            // 마지막에 소속되었던 PreviousParent의 자식으로 설정하고, 해당 위치로 설정
            transform.SetParent(previosParent);
            transform.SetSiblingIndex(3);
        }
        transform.position = previosParent.position;

        // pointerDrag는 현재 드래그 하고 있는 대상 아이템
        if (eventData.pointerDrag != null)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("InventoryItemSlot"))
                {
                    CharacterManager.Instance.inventory.ChangeItemSlot(inventoryitemSlot, result.gameObject.GetComponent<InventoryItemSlot>());
                    return;
                }
                else if (result.gameObject.CompareTag("HandItemSlot"))
                {
                    CharacterManager.Instance.inventory.ChangeItemSlot(handItemSlot.inventoryItemSlot, result.gameObject.GetComponent<HandItemSlot>().inventoryItemSlot);
                    return;
                }
            }
        }
    }
}
