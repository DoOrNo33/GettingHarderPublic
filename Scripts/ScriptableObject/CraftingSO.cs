using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Catalog", menuName = "New Catalog")]
public class CraftingSO : ScriptableObject
{
    [Header("Infoes")]
    public int CraftID;
    public ItemSO craftItem;
    public ItemSO[] requirementInfo;
    public int[] requirementCount;
    public float craftDuration = 0f;
    public int craftSortOrder = 0;

    [Header("BluePrint")]
    public bool isBuilding;
    public GameObject bluePrint;

    [Header("FX")]
    public AudioClip craft_sound;

    public bool CheckedType(ItemSO item)
    {
        if (item.itemType == Define.CraftType.Building)
        {
            return true;
        }
        return false;
    }
}
