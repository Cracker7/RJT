using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (캐릭터)
    public Vector3 offset = new Vector3(0, 10, 0); // 카메라 위치 조정
    public float smoothSpeed = 5f; // 부드러운 이동 속도

    void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산
        Vector3 targetPosition = target.position + offset;

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 캐릭터를 바라보도록 설정
        transform.LookAt(target);
    }
}
