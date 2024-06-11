using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private static readonly int isWalk = Animator.StringToHash("Walk");
    private static readonly int isRun = Animator.StringToHash("Run");
    private static readonly int isSleep = Animator.StringToHash("Sleep");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnWalkAnim()
    {
        animator.SetBool(isWalk, true);
    }

    public void OutWalkAnim()
    {
        animator.SetBool(isWalk, false);
    }

    public void OnRunAnim()
    {
        animator.SetBool(isRun, true);
    }

    public void OutRunAnim()
    {
        animator.SetBool(isRun, false);
    }

    public void OnSleep()
    {
        animator.SetBool(isSleep, CharacterManager.Instance.Player.condition.isSleep  ? false : true);
    }

    public void SetAttackAnim()
    {
        int randomAttackIndex = Random.Range(0, 3);
        switch (randomAttackIndex)
        {
            case 0:
                animator.SetTrigger("Attack1");
                break;
            case 1:
                animator.SetTrigger("Attack2");
                break;
            case 2:
                animator.SetTrigger("Attack3");
                break;
        }
    }

    public void SetPickAnim()
    {
        animator.SetTrigger("Attack3");
    }

    public void SetAxeAnim()
    {
        animator.SetTrigger("Attack2");
    }

    public void SetDrinkAnim()
    {
        animator.SetTrigger("Drink");
    }

    public void SetDeathAnim()
    {
        animator.SetTrigger("Death");
    }
}
