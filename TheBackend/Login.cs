using BackEnd;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : LoginBase
{
    [Header("InputField")]
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private TMP_InputField inputFieldPW;

    [Header("Button")]
    [SerializeField] private Button btnLogin;
    public void ClearTxt() 
    {
        inputFieldID.text = string.Empty;
        inputFieldPW.text = string.Empty;
        ResetUI();
    }
    public void OnClickLogin()
    {
        ResetUI();

        if (isFieldDataEmpty(inputFieldID.text, "아이디", 0)) { return; }
        if (isFieldDataEmpty(inputFieldPW.text, "비밀번호", 1)) { return; }

        btnLogin.interactable = false; // 한번만 클릭하도록

        StartCoroutine(nameof(LoginProcess));

        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    private void ResponseToLogin(string ID, string PW)
    {
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProcess));

            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}님 환영합니다.");
                Invoke("ChangeScene", 2f); 
            }
            else
            {
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401: // 존재하지 않는 아이디, 잘못된 비밀번호
                        message = callback.GetMessage().Contains("customID") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호 입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }
                // StatusCode 401에서 잘못된 비밀번호 일때
                if (message.Contains("비밀번호"))
                {
                    GuidForIncorrectlyEnteredData(message, 1);
                }
                else
                {
                    GuidForIncorrectlyEnteredData(message, 0);
                }
            }
        });
    }

    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"로그인 중입니다... {time:F1}");

            yield return null;
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
