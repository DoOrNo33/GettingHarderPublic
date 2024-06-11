using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Equip[] curEquip;
    public Transform[] equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    private void Start()
    {
        curEquip = new Equip[4];
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemSO data, int index)
    {
        UnEquip(index);
        curEquip[index] = Instantiate(data.equipPrefab, equipParent[index]).GetComponent<Equip>();
    }

    public void UnEquip(int index)
    {
        if(curEquip[index] != null)
        {
            Destroy(curEquip[index].gameObject);
            curEquip[index] = null;
        }
    }
}
