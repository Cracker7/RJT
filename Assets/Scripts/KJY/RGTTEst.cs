using UnityEngine;

public class RGTTEst : MonoBehaviour
{
    public Rigidbody boxRigidbody; // ���簢�� ��ü�� Rigidbody
    public float gravityForce = 50f; // ���� �Ʒ��� �̲������� �߷� ȿ�� ��ȭ
    public float lateralForce = 10f; // �¿� �̵� ��
    public float maxSpeed = 10f; // �ִ� �̵� �ӵ� ����

    void Start()
    {
        // Rigidbody�� ���� ����
        boxRigidbody.useGravity = false; // �⺻ �߷� ��� Ŀ���� �߷� ���
    }

    void FixedUpdate()
    {
        // �߷� �������� ���� ���� �̲������� ����
        Vector3 customGravity = new Vector3(0, -1, -1).normalized * gravityForce;
        boxRigidbody.AddForce(customGravity, ForceMode.Acceleration);

        // �¿� �Է�(A, D Ű �Ǵ� ����Ű)
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 lateralMovement = Vector3.right * horizontalInput * lateralForce;

        // �¿� �� ����
        boxRigidbody.AddForce(lateralMovement, ForceMode.Force);

        // �ִ� �ӵ� ����
        LimitSpeed();
    }

    // �ӵ��� �����ϴ� �Լ�
    private void LimitSpeed()
    {
        if (boxRigidbody.linearVelocity.magnitude > maxSpeed)
        {
            boxRigidbody.linearVelocity = boxRigidbody.linearVelocity.normalized * maxSpeed;
        }
    }
}