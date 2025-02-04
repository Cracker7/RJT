using UnityEngine;

/// <summary>
/// 실제 물리 적용 없이 차량의 시각적 기울어짐(롤링)만 보정하는 스크립트
/// </summary>
public class AntiRollBar : MonoBehaviour
{
    [SerializeField] private Transform body;  // 차체(Body) 모델 Transform
    [SerializeField] private WheelCollider leftWheel;
    [SerializeField] private WheelCollider rightWheel;

    [SerializeField] private float maxTiltAngle = 10f;  // 최대 기울기 (Roll)
    [SerializeField] private float tiltMultiplier = 5f; // 기울기 민감도

    private void Update()
    {
        float travelL = GetWheelTravel(leftWheel);
        float travelR = GetWheelTravel(rightWheel);

        // 좌우 바퀴 서스펜션 차이를 기반으로 롤링 계산
        float tiltAngle = (travelL - travelR) * tiltMultiplier;
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);

        // 차체(Body) 기울이기 (Vector3.right 기준으로 롤링 적용)
        body.localRotation = Quaternion.Euler(0, 0, tiltAngle);
    }

    // 서스펜션 압축 정도(바퀴의 상하 이동량) 계산
    private float GetWheelTravel(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            return (-wheel.transform.InverseTransformPoint(hit.point).y - wheel.radius) / wheel.suspensionDistance;
        }
        return 0f; // 땅에 닿지 않은 경우 기울어짐 없음
    }
}
