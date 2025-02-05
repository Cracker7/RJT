using UnityEngine;

public class AntiRollPlus : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheel;
    [SerializeField] private WheelCollider frontRightWheel;
    [SerializeField] private WheelCollider rearLeftWheel;
    [SerializeField] private WheelCollider rearRightWheel;

    [Header("정렬 설정")]
    [SerializeField] private float alignmentSpeed = 10f;
    [SerializeField] private bool showDebugRays = true;  // 디버그 레이 표시 여부

    private void Update()
    {
        // 각 바퀴 위치에서의 지면 노멀 얻기
        Vector3 flNormal = GetWheelSurfaceNormal(frontLeftWheel);
        Vector3 frNormal = GetWheelSurfaceNormal(frontRightWheel);
        Vector3 rlNormal = GetWheelSurfaceNormal(rearLeftWheel);
        Vector3 rrNormal = GetWheelSurfaceNormal(rearRightWheel);

        // 모든 바퀴가 지면에 닿아있는지 확인
        if (flNormal != Vector3.zero && frNormal != Vector3.zero &&
            rlNormal != Vector3.zero && rrNormal != Vector3.zero)
        {
            // 평균 노멀 계산
            Vector3 averageNormal = (flNormal + frNormal + rlNormal + rrNormal).normalized;

            // 타겟 회전 계산
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;

            // 부드럽게 회전 적용
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * alignmentSpeed);
        }
    }

    private Vector3 GetWheelSurfaceNormal(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            if (showDebugRays)
            {
                Debug.DrawRay(hit.point, hit.normal * 1f, Color.green);
            }
            return hit.normal;
        }
        return Vector3.zero;
    }

}
