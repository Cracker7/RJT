using UnityEngine;

public class SmoothMovementController : MonoBehaviour
{
    [Header("Rolling Settings")]
    [SerializeField] private float rollForce = 10f;         // �⺻ �������� ��
    [SerializeField] private float sideForce = 5f;          // �¿� �̵� ��
    [SerializeField] private float maxSideSpeed = 8f;       // �ִ� �¿� �ӵ�
    [SerializeField] private float groundCheckRadius = 0.5f; // ���� üũ �ݰ�

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50f;  // ȸ�� �ӵ� ���� ����
    }

    private void FixedUpdate()
    {
        CheckGround();
        if (!isGrounded) return;

        // �׻� ������(�Ʒ���) �������� �� ����
        rb.AddForce(Vector3.forward * rollForce, ForceMode.Force);

        // �¿� �Է¿� ���� �� ����
        float horizontalInput = Input.GetAxis("Horizontal");

        // ���� �¿� �ӵ� üũ
        Vector3 lateralVelocity = Vector3.right * Vector3.Dot(rb.linearVelocity, Vector3.right);

        // �ִ� �ӵ��� ���� �ʾ��� ���� �߰� �� ����
        if (Mathf.Abs(lateralVelocity.x) < maxSideSpeed ||
            Mathf.Sign(horizontalInput) != Mathf.Sign(lateralVelocity.x))
        {
            Vector3 movement = Vector3.right * horizontalInput * sideForce;
            rb.AddForce(movement, ForceMode.Force);
        }

        // ���� ������ ��ũ ���� (�ð��� ȿ��)
        rb.AddTorque(Vector3.right * rb.linearVelocity.z, ForceMode.Force);
        rb.AddTorque(Vector3.forward * -rb.linearVelocity.x, ForceMode.Force);
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(
            transform.position - Vector3.up * 0.5f, // �ణ �Ʒ����� üũ
            groundCheckRadius,
            LayerMask.GetMask("Ground")
        );
    }

    // ����׿� �Ķ���� ���� �޼���
    public void SetRollForce(float force) => rollForce = force;
    public void SetSideForce(float force) => sideForce = force;
    public void SetMaxSideSpeed(float speed) => maxSideSpeed = speed;
}
