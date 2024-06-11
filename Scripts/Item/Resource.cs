using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;

    public ItemSO itemToGive;
    public int capacy;
    public LayerMask floorLayerMask;

    public float fallDuration = 2f;
    private bool isFalling = false;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal, int damage)
    {
        int hits = Mathf.Min(damage, capacy);

        for (int i = 0; i < hits; i++)
        {
            if (capacy <= 0) break;
            capacy -= 1;
            Vector3 spawnPosition = hitPoint + Vector3.up * 0.5f;
            Vector3 dropPosition = GetDropPosition(spawnPosition);
            GameObject item = Instantiate(itemToGive.itemPrefab, dropPosition, Quaternion.identity);
        }

        if(resourceType == ResourceType.Tree)
        {
            AudioManager.Instance.HitWood();
        }
        else if(resourceType == ResourceType.Rock)
        {
            AudioManager.Instance.HitStone();
        }

        if (capacy <= 0 && !isFalling && resourceType == ResourceType.Tree)
        {
            StartCoroutine(FallTree(hitPoint));
        }
        else if (capacy <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private Vector3 GetDropPosition(Vector3 spawnPosition)
    {
        RaycastHit hit;
        float maxDistance = 10f;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, maxDistance, floorLayerMask))
        {
            return hit.point;
        }
        return spawnPosition;
    }

    private IEnumerator FallTree(Vector3 hitPoint)
    {
        isFalling = true;
        Vector3 fallDirection = (transform.position - hitPoint).normalized;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(Vector3.down, fallDirection);

        float elapsedTime = 0f;
        while (elapsedTime < fallDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / fallDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        gameObject.SetActive(false);
    }
}
