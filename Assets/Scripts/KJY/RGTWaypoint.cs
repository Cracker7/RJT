using UnityEngine;
using UnityEngine.AI;

public class RGTWaypoint : MonoBehaviour
{
    [SerializeField] private Transform[] positions; // 사용할 위치 배열
    [SerializeField] private GameObject obj; // 이동할 오브젝트
    [SerializeField] private float speed = 10f; // 이동 속도

    private int currentIndex = 0; // 현재 목표 위치 인덱스
    private Vector3 targetPosition; // 목표 위치

    void Start()
    {
        // 초기 목표 위치 설정
        SetNextTargetPosition();
    }

    void Update()
    {
        MoveToNextPosition();
    }

    // 목표 위치로 이동하는 함수
    void MoveToNextPosition()
    {
        // 부드럽게 목표 위치로 이동
        obj.transform.position = Vector3.MoveTowards(
            obj.transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // 목표 위치에 도달하면 다음 위치 설정
        if (Vector3.Distance(obj.transform.position, targetPosition) < 0.1f)
        {
            SetNextTargetPosition();
        }
    }

    // 다음 목표 위치 설정 함수
    void SetNextTargetPosition()
    {
        currentIndex = (currentIndex + 1) % positions.Length;
        targetPosition = positions[currentIndex].position;
    }
}
