using UnityEngine;
using System;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemSO : ScriptableObject
{
    [Header("Infoes")]
    public int itemID;
    public Sprite itemIcon;
    public Define.CraftType itemType;
    public string itemName;
    public string itemDescription;
    public GameObject itemPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("AddStat")]
    //

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}

[Serializable]
public class ItemDataConsumable
{
    public Define.ConsumableType consumableType;
    public float value;
}