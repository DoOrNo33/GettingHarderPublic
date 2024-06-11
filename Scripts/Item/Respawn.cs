using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Transform spawnPoint;   // 오브젝트 초기 위치
    public float CheckRadius;       // 검사할 반경

    private void Awake()
    {
        spawnPoint = transform;
    }

    private void OnEnable()
    {
        OnRespawn();
    }


    public void OnRespawn()
    {
        // 게임 오브젝트가 활성화 상태라면 생성 취소
        if (gameObject.activeSelf)
        {
            return;
        }

        // 반경 내에 다른 오브젝트가 있는지 검사
        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, CheckRadius);

        foreach (var obj in colliders)
        {
            if (obj.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                
            }
            else
            {
                return;
            }
        }

        gameObject.SetActive(true);
    }
}