using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject pausePanel;

    public void OnPausePanel()
    {
        pausePanel.SetActive(true);
    }
}
