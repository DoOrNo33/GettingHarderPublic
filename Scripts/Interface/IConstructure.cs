using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConstructure
{
    public void OnInteraction();
    public void OnTriggerEnter(Collider other);
    public void OnTriggerExit(Collider other);
}
