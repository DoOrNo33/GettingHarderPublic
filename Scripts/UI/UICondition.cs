using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition thirst;

    public TextMeshProUGUI hpText;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    private void Update()
    {
        hpText.text = health.GetHpValue();
    }
}
