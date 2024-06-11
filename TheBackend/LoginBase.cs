using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginBase : MonoBehaviour
{
    [Header("ServerMessage")]
    [SerializeField] private TextMeshProUGUI textMessage;

    [Header("PlaceHolder")]
    [SerializeField] private TextMeshProUGUI placeHolderID;
    [SerializeField] private TextMeshProUGUI placeHolderPW;
    [SerializeField] private TextMeshProUGUI placeHolderCheckPW;
    [SerializeField] private TextMeshProUGUI placeHolderEmail;

    protected void ResetUI()
    {
        textMessage.text = string.Empty;
        textMessage.color = Color.white;
        placeHolderID.text = "EnterID";
        placeHolderID.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 128f / 255f);
        placeHolderPW.text = "PassWord";
        placeHolderPW.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 128f / 255f);
        if (!placeHolderCheckPW)
        {
            return;
        }
        placeHolderCheckPW.text = "Check PassWord";
        placeHolderCheckPW.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 128f / 255f);
        placeHolderEmail.text = "Email";
        placeHolderEmail.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 128f / 255f);
    }

    protected void SetMessage(string msg)
    {
        textMessage.text = msg;
        textMessage.color = Color.white;
    }

    protected void GuidForIncorrectlyEnteredData(string msg, int input)
    {
        switch (input)
        {
            case 0:
                placeHolderID.text = msg;
                placeHolderID.color = Color.red;
                break;
            case 1:
                placeHolderPW.text = msg;
                placeHolderPW.color = Color.red;
                break;
            case 2:
                placeHolderCheckPW.text = msg;
                placeHolderCheckPW.color = Color.red;
                break;
            case 3:
                placeHolderEmail.text = msg;
                placeHolderEmail.color = Color.red;
                break;
        }
        textMessage.text = msg;
        textMessage.color = Color.red;
    }

    protected bool isFieldDataEmpty(string field, string result, int input)
    {
        if (field.Trim().Equals(""))
        {
            GuidForIncorrectlyEnteredData($"{result} 필드를 채워주세요", input);
            return true;
        }
        return false;
    }
}
