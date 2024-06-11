using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    public GameObject startTxt;
    public GameObject loginPanel;
    public void OnStartTxt()
    {
        startTxt.SetActive(true);
    }

    public void PressStartTxt()
    {
        loginPanel.SetActive(true);
    }
}
