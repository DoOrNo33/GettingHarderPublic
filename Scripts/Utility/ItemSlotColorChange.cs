using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemSlotColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;

    private void Awake()
    {
        Transform firstChild = transform.GetChild(1);
        image = firstChild.GetComponent<Image>();
    }
    // 마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow;
    }
    // 마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}
