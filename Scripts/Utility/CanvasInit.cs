using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInit : MonoBehaviour
{
    [SerializeField]
    GameObject ConstructurePanel;
    [SerializeField]
    GameObject CraftingPanel;
    [SerializeField]
    GameObject CautionTxt;
    [SerializeField]
    GameObject InteractionTxt;
    [SerializeField]
    GameObject SleepBtn;
    // Start is called before the first frame update
    void Start()
    {
        if(CraftingPanel == null) 
        {
            Init();
        }
    }

    public void Init()
    {
        UIManager.Instance.CraftPanel = CraftingPanel;
        UIManager.Instance.CautionTxt = CautionTxt;
        UIManager.Instance.ConstructurePanel = ConstructurePanel;
        UIManager.Instance.interactionTxt = InteractionTxt;
        UIManager.Instance.Panels[0] = SleepBtn;
        UIManager.Instance.Panels[1] = CraftingPanel;
    }
}
