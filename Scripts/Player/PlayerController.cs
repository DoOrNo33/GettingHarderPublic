using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float curSpeed;
    private float runSpeed;
    public float runStamina;
    private Vector2 curMovementInput;
    public bool canRun;
    private Coroutine runCoroutine;

    private Rigidbody rigidbody;
    [HideInInspector]
    public PlayerAnimation anim;

    [Header("Interact")]
    public Camera camera;
    public float attackRange;
    public LayerMask interactableLayer;

    public Action inventory;
    public UnityEvent CallbackInteraction;
    private Coroutine recoveryHP;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<PlayerAnimation>();
        camera = Camera.main;
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        curSpeed = moveSpeed;
        runSpeed = moveSpeed * 2;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 dir = new Vector3(curMovementInput.x, 0, curMovementInput.y).normalized;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    private void Rotate()
    {
        if (curMovementInput != Vector2.zero)
        {
            Vector3 dir = new Vector3(curMovementInput.x, 0, curMovementInput.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * moveSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
            anim.OnWalkAnim();

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            anim.OutWalkAnim();
        }
        else if (context.started)
        {
            if (CharacterManager.Instance.Player.condition.isSleep)
            {
                OnWakeUp();
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && CanRun())
        {
            anim.OnRunAnim();
            moveSpeed = runSpeed;
            runCoroutine = StartCoroutine(RunStamina());

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            StopRunning();
        }
    }

    bool CanRun()
    {
        return CharacterManager.Instance.Player.condition.uiCondition.stamina.uiBar.fillAmount > 0.01f;
    }

    private IEnumerator RunStamina()
    {
        while (CanRun())
        {
            CharacterManager.Instance.Player.condition.UseStamina(runStamina);
            yield return null;
        }
        StopRunning();
    }

    private void StopRunning()
    {
        anim.OutRunAnim();
        moveSpeed = curSpeed;
        StopCoroutine(runCoroutine);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (CraftManager.Instance.isBuilding)
        {
            if (context.canceled && !EventSystem.current.IsPointerOverGameObject())
            {
                CraftManager.Instance.Build();
            }
            return;
        }
        if (context.phase == InputActionPhase.Performed && CharacterManager.Instance.Player.equipment.curEquip[0] != null && !EventSystem.current.IsPointerOverGameObject())
        {
            CharacterManager.Instance.Player.equipment.curEquip[0].OnAttackInput();
        }
        if(context.phase == InputActionPhase.Started && !EventSystem.current.IsPointerOverGameObject())
        {
            OnPlayerCheckRay();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (CraftManager.Instance.isBuilding && context.canceled)
        {
            CraftManager.Instance.CanceledBuild();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
        }
    }

    public void OnSleep()
    {
        anim.OnSleep();
        CharacterManager.Instance.Player.condition.isSleep = true;
        recoveryHP = StartCoroutine(CharacterManager.Instance.Player.condition.IncreaseHp(1));
    }

    public void OnWakeUp()
    {
        anim.OnSleep();
        CharacterManager.Instance.Player.condition.isSleep = false;
        StopCoroutine(recoveryHP);
    }


    public void OnPlayerCheckRay()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, hit.point);

            if ((hit.collider.CompareTag("Water") && distanceToPlayer <= attackRange))
            {
                anim.SetDrinkAnim();
                CharacterManager.Instance.Player.condition.Drink(20);
            }
            else if (hit.collider.TryGetComponent(out Resource resource) && distanceToPlayer <= 1)
            {
                if(resource.resourceType == Define.ResourceType.Crop)
                {
                    anim.SetDrinkAnim();
                    resource.Gather(hit.point, hit.normal, 1);
                }
            }
        }
    }

    public void OnAttackHitEvent()
    {
        // 현재 장착된 장비를 가져와서 OnHit 호출
        if (CharacterManager.Instance.Player.equipment.curEquip[0] is EquipTool equipTool)
        {
            OnHit(equipTool);
        }
    }

    // OnHit 메서드
    public void OnHit(EquipTool equipTool)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, hit.collider.transform.position);

            if (equipTool.doesGatherResources && hit.collider.TryGetComponent(out Resource resource) && distanceToPlayer <= attackRange)
            {
                if (resource.resourceType == equipTool.resourceType)
                {
                    resource.Gather(hit.point, hit.normal, equipTool.damage);
                }
            }
            else if (equipTool.doesDealDamage && (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Animal")) && distanceToPlayer <= attackRange)
            {
                if (hit.collider.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakePhysicalDamage(equipTool.damage);
                }
            }
        }
    }


    public void OnInteraction(InputAction.CallbackContext context)
    {
        CallbackInteraction?.Invoke();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Pause.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.OnPauseEvent(!GameManager.Instance.IsPaused);
        }
    }
}
