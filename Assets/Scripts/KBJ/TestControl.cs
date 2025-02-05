using UnityEngine;

public class RollingController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveForce = 10f;  // 가속력
    public float turnSpeed = 2f;   // 회전 속도
    public float brakeForce = 8f;  // 감속력

    private Vector3 forwardDirection; // 현재 전진 방향

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        forwardDirection = transform.forward; // 초기 방향 설정
    }

    void FixedUpdate()
    {
        // 디버그 라인 (현재 전진 방향 시각화)
        Debug.DrawLine(transform.position, transform.position + forwardDirection * 5f, Color.green);

        // W키: 현재 forwardDirection 방향으로 가속
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(forwardDirection * moveForce, ForceMode.Acceleration);
        }

        // S키: 진행 방향의 반대 방향으로 감속
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-forwardDirection * brakeForce, ForceMode.Acceleration);
        }

        // A/D키: 회전 (forwardDirection을 조정)
        if (Input.GetKey(KeyCode.A))
        {
            forwardDirection = Quaternion.Euler(0, -turnSpeed, 0) * forwardDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            forwardDirection = Quaternion.Euler(0, turnSpeed, 0) * forwardDirection;
        }
    }
}
