using UnityEngine;
using UnityEngine.AI;

public class RGTWaypoint : MonoBehaviour
{
    public Transform[] waypoints; // ��������Ʈ �迭
    private int currentWaypointIndex = 0; // ���� ��ǥ ��������Ʈ

    private NavMeshAgent agent;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        agent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    void Update()
    {
        // �������� ���� �����ϸ� ���� ��������Ʈ�� �̵�
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // ���� ��������Ʈ �ε��� ���� (��ȯ ���)
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}
