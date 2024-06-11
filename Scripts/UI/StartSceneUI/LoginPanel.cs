using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    [Header("Button")]
    public GameObject loginBtn;
    public GameObject createBtn;

    [Header("InputField")]
    public GameObject loginField;
    public GameObject createField;

    [Header("Text")]
    public TextMeshProUGUI logoPanelTxt;
    public TextMeshProUGUI serverMessageTxt;

    private Login login;
    private RegisterAccount create;
    private void Start()
    {
        login = GetComponent<Login>();
        create = GetComponent<RegisterAccount>();
    }
    public void Login()
    {
        login.ClearTxt();
        logoPanelTxt.text = "로그인";
        serverMessageTxt.text = "로그인이 필요합니다";

        if (createBtn)
        {
            createBtn.SetActive(false);
            createField.SetActive(false);
        }
        loginField.SetActive(true);
        loginBtn.SetActive(true);
    }

    public void Create()
    {
        create.ClearTxt();
        logoPanelTxt.text = "계정 생성";
        serverMessageTxt.text = "계정을 생성해주세요";

        if (loginBtn )
        {
            loginBtn.SetActive(false);
            loginField.SetActive(false);
        }
        createBtn.SetActive(true);
        createField.SetActive(true);
    }
}
