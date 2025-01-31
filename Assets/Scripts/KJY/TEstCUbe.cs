using UnityEngine;

public class TEstCUbe : MonoBehaviour
{
    private Rigidbody rb;
    public float rollForce = 10f;  // 굴러가는 힘
    public float moveForce = 5f;   // 좌우 이동 힘

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 10f; // 최대 회전 속도 제한
    }

    void FixedUpdate()
    {
        ApplyRolling();
        HandleInput();
    }

    void ApplyRolling()
    {
        // 경사면을 감지하여 방향을 계산
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal);
            rb.AddTorque(Vector3.Cross(slopeDirection, Vector3.up) * rollForce);
            Debug.Log("Torque : " + Vector3.Cross(slopeDirection, Vector3.up)*rollForce);
        }
    }

    void HandleInput()
    {
        float moveInput = Input.GetAxis("Horizontal"); // A, D 또는 좌우 화살표 입력 받기
        rb.AddForce(Vector3.right * moveInput * moveForce, ForceMode.Acceleration);
    }


}
