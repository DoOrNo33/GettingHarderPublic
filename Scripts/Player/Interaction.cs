using UnityEngine;
using System.Collections.Generic;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private List<GameObject> interactableObjects = new List<GameObject>(); // 상호작용 가능한 오브젝트 리스트
    private List<IInteractable> interactables = new List<IInteractable>(); // 상호작용 가능한 인터페이스 리스트

    private new Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        // 일정 시간마다 상호작용 가능한 객체를 체크
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 플레이어 주변 범위 내의 콜라이더 탐색
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxCheckDistance, layerMask);

            interactableObjects.Clear(); // 이전 리스트 초기화
            interactables.Clear(); // 이전 리스트 초기화

            // 상호작용 가능한 객체 찾기
            foreach (var hitCollider in hitColliders)
            {
                IInteractable interactable = hitCollider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactableObjects.Add(hitCollider.gameObject);
                    interactables.Add(interactable);
                }
            }
        }
    }

    // 상호작용 입력을 처리하는 메서드
    public void OnInteractInput()
    {
        if (interactables.Count > 0)
        {
            // 가장 가까운 아이템부터 상호작용
            IInteractable interactable = interactables[0];
            interactable.OnInteract();

            // 상호작용한 아이템 제거
            interactables.RemoveAt(0);
            interactableObjects.RemoveAt(0);
        }
    }
}
