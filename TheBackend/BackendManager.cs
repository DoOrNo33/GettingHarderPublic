using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    public static BackendManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //뒤끝 서버 초기화
        BackendSetup();
    }

    private void Update()
    {
        // 서버의 비동기 메서드 호출(콜백 함수 풀링)을 위해 작성
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    private void BackendSetup()
    {
        var backend = Backend.Initialize(true);

        if (backend.IsSuccess())
        {
            Debug.Log($"초기화 성공 : {backend}");
        }
        else
        {
            Debug.Log($"실패 : {backend}");
        }
    }
}
