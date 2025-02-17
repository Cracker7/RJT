using UnityEngine;
using UnityEngine.AI;

public class RGTWaypoint : MonoBehaviour
{
    public Transform[] waypoints; // 웨이포인트 배열
    private int currentWaypointIndex = 0; // 현재 목표 웨이포인트

    private NavMeshAgent agent;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        agent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    void Update()
    {
        // 목적지에 거의 도착하면 다음 웨이포인트로 이동
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

        // 다음 웨이포인트 인덱스 설정 (순환 방식)
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}
