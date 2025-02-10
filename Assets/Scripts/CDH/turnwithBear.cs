using UnityEngine;

public class turnwithBear : MonoBehaviour
{
    public Transform tr;
    // 회전 속도를 설정합니다 (단위: 도/초).
    public float rotationSpeed = 100f;

    void LateUpdate()
    {
        // Y축을 기준으로 회전합니다.
        tr.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
