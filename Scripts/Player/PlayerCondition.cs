using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }
    Condition thirst { get { return uiCondition.thirst; } }

    public float noHealthDecay;

    private bool isDead = false;
    public bool isSleep = false;
    private void Update()
    {
        if (isDead) return;

        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        thirst.Subtract(thirst.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue == 0f || thirst.curValue == 0f)
        {
            health.Subtract(noHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            isDead = true;
            CharacterManager.Instance.Player.controller.anim.SetDeathAnim();
            GameManager.Instance.OnGameOverEvent();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public IEnumerator IncreaseHp(float amount)
    {
        while (isSleep)
        {
            Heal(amount);
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        thirst.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount <= 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
    }

    public float curHealth() { return health.curValue; }
    public float curHunger() { return hunger.curValue; }
    public float curStamina() { return stamina.curValue; }
    public float curThirst() {  return thirst.curValue; }

    public void setHealth(float value) { health.curValue = value; }
    public void setHunger(float value) { hunger.curValue = value; }
    public void setStamina(float value) { stamina.curValue = value; }
    public void setThirst(float value) { thirst.curValue = value; }
}
