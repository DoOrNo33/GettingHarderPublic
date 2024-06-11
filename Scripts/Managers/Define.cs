using UnityEngine;

public class Define : MonoBehaviour
{
    public enum ConsumableType
    {
        Hunger,
        Health
    }

    public enum CraftType
    {
        None,
        Building,
        Tool,
        Weapon,
        Food,        
        Prop,
        Wearable,
        Resource,
        MaxCount
    }

    public enum AnimalState
    {
        Idle,
        Wandering,
        Attacking,
        Fleeing
    }

    public enum AnimalType
    {
        AttackOnSight,
        AttackOnHit,
        FleeOnSight,
        FleeOnHit
    }

    public enum ResourceType
    {
        Tree,
        Rock,
        Crop
    }

    public enum Constructure
    {
        Craft,
        Sleep,
        None
    }

    public enum SaveConID
    {
        Sleep = 501,
        Craft,       
        None
    }
}
