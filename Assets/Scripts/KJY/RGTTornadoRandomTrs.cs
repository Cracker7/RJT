using UnityEngine;
using UnityEngine.AI;

public class RGTTornadoRandomTrs : MonoBehaviour
{
    [SerializeField] float UpdateInterval = 3f;
    private NavMeshAgent agent;
    private float TimeSinceLastUpdate;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        TimeSinceLastUpdate = UpdateInterval;
    }

    private void Update()
    {
        TimeSinceLastUpdate += Time.deltaTime; // �ð� ���� �����մϴ�.

        if (TimeSinceLastUpdate >= UpdateInterval) // ������ �ð� ������ �������� Ȯ���մϴ�.
        {
            Vector3 randomPosition = GetRandomPositionOnNavMesh(); // NavMesh ���� ������ ��ġ�� �����ɴϴ�.
            agent.SetDestination(randomPosition); // NavMeshAgent�� ��ǥ ��ġ�� ���� ��ġ�� �����մϴ�.
            TimeSinceLastUpdate = 0f; // �ð� ���� �ʱ�ȭ�մϴ�.
        }
    }

    Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 100f; // ���ϴ� ���� ���� ������ ���� ���͸� �����մϴ�.
        randomDirection += transform.position; // ���� ���� ���͸� ���� ��ġ�� ���մϴ�.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 100f, NavMesh.AllAreas)) // ���� ��ġ�� NavMesh ���� �ִ��� Ȯ���մϴ�.
        {
            return hit.position; // NavMesh ���� ���� ��ġ�� ��ȯ�մϴ�.
        }
        else
        {
            return transform.position; // NavMesh ���� ���� ��ġ�� ã�� ���� ��� ���� ��ġ�� ��ȯ�մϴ�.
        }
    }


}
