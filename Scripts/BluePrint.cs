using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> list = new List<GameObject>();
    [SerializeField]
    LayerMask layer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            if (list.Count == 0)
            {
                CraftManager.Instance.CheckedCanBuilding(this.gameObject, false);
            }
            list.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            list.Remove(other.gameObject);
            if (list.Count == 0)
            {
                CraftManager.Instance.CheckedCanBuilding(this.gameObject, true);
            }
        }
    }
}
