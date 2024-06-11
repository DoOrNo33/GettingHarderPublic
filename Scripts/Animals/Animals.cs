using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Animals : MonoBehaviour, IDamagable
{
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Run = Animator.StringToHash("Run");

    [Header("AI")]
    private NavMeshAgent agent;
    private Define.AnimalState animalState;
    public AnimalData animalData;
    private Vector3 startingPos;

    [Header("Combat")]
    public int health;
    private float lastAttackTime;
    private float playerDistance;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

   private void Awake()
    {
        health = animalData.health;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        SetState(Define.AnimalState.Wandering);
        startingPos = transform.position;
    }


    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);


        animator.SetBool(Move, animalState != Define.AnimalState.Idle);

        switch (animalState)
        {
            case Define.AnimalState.Idle:
            case Define.AnimalState.Wandering:
                PassiveUpdate();
                break;
            case Define.AnimalState.Attacking:
                AttackingUpdate();
                break;
            case Define.AnimalState.Fleeing:
                FleeingUpdate();
                break;
        }
    }
    
    void SetState(Define.AnimalState state)
    {
        animalState = state;

        switch (animalState)
        {
            case Define.AnimalState.Idle:
                agent.speed = animalData.walkSpeed;
                agent.isStopped = true;
                break;
            case Define.AnimalState.Wandering:
                agent.speed = animalData.walkSpeed;
                agent.isStopped = false;
                break;
            case Define.AnimalState.Attacking:
                agent.speed = animalData.runSpeed;
                agent.isStopped = false;
                break;
            case Define.AnimalState.Fleeing:
                agent.speed = animalData.runSpeed;
                agent.isStopped = false;
                break;
        }

        //animator.speed = agent.speed / animalData.walkSpeed;
    }

    void PassiveUpdate()
    {
        if (Vector3.Distance(transform.position, startingPos) > animalData.homeAreaRadius)  // 시작 위치에서 일정 범위를 벗어나면 집으로 돌아감
        {
            agent.SetDestination(startingPos);
        }
        else if (animalState == Define.AnimalState.Wandering && agent.remainingDistance <= 0.1f)
        {
            SetState(Define.AnimalState.Idle);
            Invoke("WanderToNewLocation", Random.Range(animalData.minWanderWaitTime, animalData.maxWanderWaitTime));
        }

        if (playerDistance < animalData.detectDistance)
        {
            switch (animalData.animalType)
            {
                case Define.AnimalType.AttackOnSight:
                    if (animalState != Define.AnimalState.Attacking)
                    {
                        SetState(Define.AnimalState.Attacking);
                    }
                    break;
                case Define.AnimalType.FleeOnSight:
                    if (animalState != Define.AnimalState.Fleeing)
                    {
                        SetState(Define.AnimalState.Fleeing);
                    }
                    break;
                case Define.AnimalType.AttackOnHit:
                case Define.AnimalType.FleeOnHit:
                    break;
            }

        }
    }

    void WanderToNewLocation()
    {
        if (animalState != Define.AnimalState.Idle) return;

        SetState(Define.AnimalState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        int i = 0;

        do
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(animalData.minWanderDistance, animalData.maxWanderDistance)), out hit, animalData.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }
        while (Vector3.Distance(transform.position, hit.position) < animalData.detectDistance);

        return hit.position;
    }

    void AttackingUpdate()
    {
        if (playerDistance < animalData.attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > animalData.attackCooldown)
            {
                Debug.Log("공격");
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.condition.GetComponent<IDamagable>().TakePhysicalDamage(animalData.damage);
                //animator.speed = 1;
                animator.SetTrigger(Attack);
            }
        }
        else 
        {
            if (playerDistance < animalData.detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(Define.AnimalState.Wandering);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(Define.AnimalState.Wandering);
            }
        }
    }

    void FleeingUpdate()
    {
        if (playerDistance < animalData.fleeDistance)
        {
            agent.isStopped = false;
            Vector3 fleeDirection = transform.position - CharacterManager.Instance.Player.transform.position;
            Vector3 newDestination = transform.position + fleeDirection;
            agent.SetDestination(newDestination);
        }
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            SetState(Define.AnimalState.Wandering);
        }
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < animalData.fieldOfView * 0.5f;
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(DamageFlash());

        switch (animalData.animalType)      // 데미지 들어왔을 때 행동
        {
            case Define.AnimalType.AttackOnHit:
                if (animalState != Define.AnimalState.Attacking)
                {
                    SetState(Define.AnimalState.Attacking);
                }
                break;
            case Define.AnimalType.FleeOnHit:
                if (animalState != Define.AnimalState.Fleeing)
                {
                    SetState(Define.AnimalState.Fleeing);
                }
                break;
            case Define.AnimalType.AttackOnSight:
            case Define.AnimalType.FleeOnSight:
                break;
        }
    }

    void Die()
    {
        for (int i = 0; i < animalData.dropOnDeath.Length; i++)
        {
            Instantiate(animalData.dropOnDeath[i].itemPrefab, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    IEnumerator DamageFlash()
    {
        Color[] originalColors = new Color[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalColors[i] = meshRenderers[i].material.color;

            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = originalColors[i];
        }
    }
}
