using UnityEngine;

public class CarStabilizer : MonoBehaviour
{
    private Rigidbody rb;
    // 최대 허용 롤(좌우 기울기) 각도 (예: 10도)
    public float maxRollAngle = 10f;
    // 회전 보정 보간 속도 (0~1 사이의 값, 1이면 즉시 보정)
    public float rotationLerpFactor = 0.1f;

    // 선택 사항: 피치(앞뒤 기울기)의 최소/최대 각도 제한
    // 언덕 주행 시 필요에 따라 허용 범위를 넓게 잡을 수 있습니다.
    public float minPitchAngle = -60f;
    public float maxPitchAngle = 60f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 현재 회전을 Euler 각도로 변환 (Unity의 Euler는 0~360이므로 -180~180로 변환하여 다룸)
        Vector3 currentEuler = rb.rotation.eulerAngles;
        float currentPitch = NormalizeAngle(currentEuler.x);
        float currentYaw = currentEuler.y;   // 회전 제어에서 별도 수정이 필요없다면 그대로 사용
        float currentRoll = NormalizeAngle(currentEuler.z);

        // 롤은 반드시 제한 (좌우 넘침 방지)
        float clampedRoll = Mathf.Clamp(currentRoll, -maxRollAngle, maxRollAngle);

        // 피치는 상황에 따라 어느 정도 허용 (여기서는 선택 사항입니다)
        float clampedPitch = Mathf.Clamp(currentPitch, minPitchAngle, maxPitchAngle);

        // 타겟 회전 생성: 피치와 롤은 클램프한 값, 요는 그대로 유지
        Quaternion targetRotation = Quaternion.Euler(clampedPitch, currentYaw, clampedRoll);

        // 부드러운 보간 적용 (Slerp 사용)
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationLerpFactor);
        rb.MoveRotation(newRotation);
    }

    // Euler 각도를 -180 ~ 180 범위로 정규화
    float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;
        return angle;
    }
}
