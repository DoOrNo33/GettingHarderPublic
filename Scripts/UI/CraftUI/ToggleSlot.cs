using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSlot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI toggleLabel;
    public RectTransform contentRectTr;
    [SerializeField]
    private ScrollRect scrollRect;

    public void ToggleInit(Define.CraftType type)
    {
        SetLabel(type);
    }

    public void ChangeContent(bool isOn)
    {
        scrollRect.content.gameObject.SetActive(false);
        if (isOn) 
        {
            contentRectTr.gameObject.SetActive(true);
            scrollRect.content = contentRectTr;
        }
    }

    public void SetLabel(Define.CraftType type)
    {
        toggleLabel.text = GetLabel(type);
    }

    string GetLabel(Define.CraftType type)
    {
        string name = string.Empty;
        switch (type)
        {
            case Define.CraftType.None:
                return name = " ";
            case Define.CraftType.Building:
                return name = "건축";
            case Define.CraftType.Tool:
                return name = "도구";
            case Define.CraftType.Weapon:
                return name = "무기";
            case Define.CraftType.Food:
                return name = "음식";
            case Define.CraftType.Prop:
                return name = "소모품";
            case Define.CraftType.Wearable:
                return name = "옷";
            case Define.CraftType.Resource:
                return name = "자원";
            default:
                return name = "에러";  
        }
    }
}
