using UnityEngine;
using UnityEngine.AI;

public class RGTWaypoint : MonoBehaviour
{
    [SerializeField] private Transform[] positions; // ����� ��ġ �迭
    [SerializeField] private GameObject obj; // �̵��� ������Ʈ
    [SerializeField] private float speed = 10f; // �̵� �ӵ�

    private int currentIndex = 0; // ���� ��ǥ ��ġ �ε���
    private Vector3 targetPosition; // ��ǥ ��ġ

    void Start()
    {
        // �ʱ� ��ǥ ��ġ ����
        SetNextTargetPosition();
    }

    void Update()
    {
        MoveToNextPosition();
    }

    // ��ǥ ��ġ�� �̵��ϴ� �Լ�
    void MoveToNextPosition()
    {
        // �ε巴�� ��ǥ ��ġ�� �̵�
        obj.transform.position = Vector3.MoveTowards(
            obj.transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // ��ǥ ��ġ�� �����ϸ� ���� ��ġ ����
        if (Vector3.Distance(obj.transform.position, targetPosition) < 0.1f)
        {
            SetNextTargetPosition();
        }
    }

    // ���� ��ǥ ��ġ ���� �Լ�
    void SetNextTargetPosition()
    {
        currentIndex = (currentIndex + 1) % positions.Length;
        targetPosition = positions[currentIndex].position;
    }
}
