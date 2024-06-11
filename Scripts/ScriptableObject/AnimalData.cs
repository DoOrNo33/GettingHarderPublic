using SurvivalEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "ScriptableObject/AnimalData", order = 1)]
public class AnimalData : ScriptableObject
{
    public int AnimalID;

    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemSO[] dropOnDeath;

    [Header("AI")]
    public float detectDistance;
    public Define.AnimalType animalType;
    public float homeAreaRadius;
    public float fleeDistance;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackCooldown;
    public float attackDistance;
    public float fieldOfView;
}
