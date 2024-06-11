using System.Collections;
using UnityEngine;
using static Define;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;
    public ResourceType resourceType;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina)) attacking = true;
            if (resourceType == ResourceType.Rock && doesGatherResources)
            {
                CharacterManager.Instance.Player.controller.anim.SetPickAnim();
            }
            else if (resourceType == ResourceType.Tree && doesGatherResources)
            {
                CharacterManager.Instance.Player.controller.anim.SetAxeAnim();
            }
            else
            {
                CharacterManager.Instance.Player.controller.anim.SetAttackAnim();
            }
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        CharacterManager.Instance.Player.controller.moveSpeed = 0;
        yield return new WaitForSeconds(attackRate);
        CharacterManager.Instance.Player.controller.moveSpeed = 5;
        attacking = false;
    }
}
