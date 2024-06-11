using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.UI;

public class RegisterAccount : LoginBase
{
    [Header("InputField")]
    [SerializeField] private TMP_InputField inputFieldID;
    [SerializeField] private TMP_InputField inputFieldPW;
    [SerializeField] private TMP_InputField inputFieldCheakPW;
    [SerializeField] private TMP_InputField inputFieldEmail;

    [Header("Button")]
    [SerializeField] private Button btnRegisterAccount;

    public void ClearTxt()
    {
        inputFieldID.text = string.Empty;
        inputFieldPW.text = string.Empty;
        inputFieldCheakPW.text = string.Empty;
        inputFieldEmail.text = string.Empty;
        ResetUI();
    }

    public void OnClickRegisterAccount()
    {
        ResetUI();

        if (isFieldDataEmpty(inputFieldID.text,"아이디", 0)) { return; }
        if (isFieldDataEmpty(inputFieldPW.text, "비밀번호", 1)) { return; }
        if (isFieldDataEmpty(inputFieldCheakPW.text, "비밀번호 확인", 2)) { return; }
        if (isFieldDataEmpty(inputFieldEmail.text, "메일 주소", 3)) { return; }

        // 비밀번호와 비밀번호 확인의 내용이 다를 때
        if (!inputFieldPW.text.Equals(inputFieldCheakPW.text))
        {
            inputFieldCheakPW.text = string.Empty;
            GuidForIncorrectlyEnteredData("비밀번호가 일치하지 않습니다.", 2);
            return;
        }

        // 메일 형식 검사
        if (!inputFieldEmail.text.Contains("@"))
        {
            inputFieldEmail.text = string.Empty;
            GuidForIncorrectlyEnteredData("메일 형식이 잘못되었습니다. (ex. address01@xx.xx)", 3);
            return;
        }

        btnRegisterAccount.interactable = false;
        SetMessage("계정 생성중입니다..");

        // 뒤끝 서버 계정 생성 시도
        CustomASignUp();
    }
    // 계정 생성 시도 후 서버로부터 전달받은 message를 기반으로 로직 처리
    private void CustomASignUp()
    {
        Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
        {
            // 계정 생성 버튼 활성화
            btnRegisterAccount.interactable = true;
            // 계정 생성 성공
            if (callback.IsSuccess())
            {
                // Email정보 업데이트
                Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        SetMessage($"계정 생성 성공.{inputFieldID.text}님 환영합니다.");
                    }
                });
            }
            // 계정 생성 실패
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 409: // 중복된 customID가 존재하는 경우
                        message = "이미 존재하는 아이디입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }
                if (message.Contains("아이디"))
                {
                    GuidForIncorrectlyEnteredData(message, 0);
                }
                else
                {
                    SetMessage(message);
                }

            }
        });
    }
}
