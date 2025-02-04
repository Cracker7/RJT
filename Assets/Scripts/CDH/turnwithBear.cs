using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    // 회전 속도를 설정합니다 (단위: 도/초).
    public float rotationSpeed = 100f;

    void Update()
    {
        // Y축을 기준으로 회전합니다.
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
