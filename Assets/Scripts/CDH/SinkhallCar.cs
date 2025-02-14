using UnityEngine;

public class SinkhallCar : MonoBehaviour
{
    public Rigidbody rb;  // ������ ������ٵ�
    public float sinkingSpeed = 0.5f;  // ����ɴ� �ӵ�
    public float sinkingThreshold = 0.3f;  // 30% �̻� ��������� �� �̻� �̵� �Ұ�
    public float maxSinkingDepth = 1f;  // �ִ� ������� ���� (1f�� �����ϸ� 100%���� �������)
    public float escapeForce = 5f;  // ��Ÿ�� �������� �� ��
    public float escapeSpeed = 2f;  // ��Ÿ�� ���� �ӵ��� �ݿ��� ����

    private bool isStuck = false;  // �������� ���ϴ� ���� ����
    private bool isEscaping = false;  // ��Ÿ�� ���������� ���� ����
    private float sinkingDepth = 0f;  // ���� ������� ����
    private float lastTapTime = 0f;  // ������ ��Ÿ �ð�
    private float minTapInterval = 0.2f;  // ��Ÿ ���� (��)

    void Update()
    {
        // �ӵ��� 60 ���Ϸ� �������� ����ɱ� ����
        if (rb.linearVelocity.magnitude < 60f && !isStuck)
        {
            // ����ɴ� ���� ����
            sinkingDepth += Time.deltaTime * sinkingSpeed;

            // ������� ������ 30%�� ������ �� �̻� �̵� �Ұ�
            if (sinkingDepth >= maxSinkingDepth * sinkingThreshold && !isStuck)
            {
                isStuck = true;
            }
        }

        // ��Ÿ �� ���� �ö� �� �ִ� ����
        if (isStuck && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            if (Time.time - lastTapTime < minTapInterval)
            {
                // ���� ��Ÿ �� ȿ��
                StartEscape(true);
            }
            else
            {
                // ��Ÿ���� ������ ������ �ӵ��� �ö��
                StartEscape(false);
            }
            lastTapTime = Time.time;  // ��Ÿ �ð� ����
        }
    }

    void FixedUpdate()
    {
        if (isEscaping)
        {
            // ��Ÿ �� ���� �ö󰡴� ȿ�� (���� ��Ÿ�ϼ��� �� ���� ��)
            float escapeMultiplier = isEscaping ? escapeSpeed : 1f;
            rb.AddForce(Vector3.up * escapeForce * escapeMultiplier, ForceMode.Acceleration);

            // ���� �ö󰡴ٰ� ���̰� 0�� �����ϸ� �� �̻� �ö��� �ʵ���
            if (sinkingDepth > 0)
            {
                sinkingDepth -= Time.deltaTime * sinkingSpeed;
            }
            else
            {
                isEscaping = false;  // Ż�� �Ϸ�
                isStuck = false;     // �ٽ� ������ �� �ְ�
            }
        }

        // �ִ� ������� ���� ����
        if (sinkingDepth > maxSinkingDepth)
        {
            sinkingDepth = maxSinkingDepth;
        }
    }

    void StartEscape(bool isFastEscape)
    {
        // ���� ��Ÿ�� �� ���� ������ �ö�
        isEscaping = true;
        if (isFastEscape)
        {
            escapeForce = 10f;  // ��Ÿ �ӵ��� ������ ���� �� ���ϰ�
        }
        else
        {
            escapeForce = 5f;   // �Ϲ� ��Ÿ �ӵ�
        }
    }
}
