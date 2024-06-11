using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject ConstructurePanel;
    public GameObject CraftPanel;
    public GameObject interactionTxt;
    public GameObject CautionTxt;
    public GameObject[] Panels;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void OnPanel(GameObject go)
    {
        go.SetActive(true);
    }

    public void HidePanel(GameObject go)
    {
        go.SetActive(false);
    }
}
