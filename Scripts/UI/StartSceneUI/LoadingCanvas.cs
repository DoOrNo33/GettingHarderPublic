using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    public GameObject loadingCanvas;
    public TextMeshProUGUI loadingPercentTxt;
    public Image greenBar;

    [SerializeField] private float time = 0f;
    [SerializeField] private float addTime = 0f;

    void Update()
    {
        addTime += Time.deltaTime;
        if (addTime >= 1f)
        {
            addTime += time;
            time += addTime;
            addTime = 0f;
        }
        greenBar.fillAmount = Percentage();
        loadingPercentTxt.text = (greenBar.fillAmount * 100).ToString("F2") + "%";

        if (greenBar.fillAmount == 1)
        {
            Invoke("EndCanvas", 1f);
        }
    }

    float Percentage()
    {
        SetTime();
        return Mathf.Clamp(time / 100, 0, 1);
    }

    void SetTime()
    {
        time += Time.deltaTime;
    }

    void EndCanvas()
    {
        loadingCanvas.SetActive(false);
    }
}
