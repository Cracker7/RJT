using System.Linq;
using UnityEngine;

public class DrawOutline : MonoBehaviour
{
    public float moveSpeed = 5f;  // 이동 속도
    public float detectionRadius = 5f; // 감지 범위
    public LayerMask layer;

    private GameObject closestObject;
    private GameObject previousClosestObject;

    void Update()
    {
        MovePlayer();
        DetectClosestObject();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");  // A, D (왼쪽, 오른쪽)
        float moveZ = Input.GetAxisRaw("Vertical");    // W, S (위, 아래)

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed * Time.deltaTime;
        if (move != Vector3.zero)
        {
            transform.Translate(move, Space.World);
        }
    }

    void DetectClosestObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, layer);

        GameObject newClosestObject = colliders
            .Select(c => c.gameObject)
            .OrderBy(go => (go.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();

        if (newClosestObject != closestObject)
        {
            UpdateOutlineEffect(newClosestObject);
            closestObject = newClosestObject;
        }
    }

    void UpdateOutlineEffect(GameObject newObject)
    {
        if (previousClosestObject != null)
        {
            RemoveOutline(previousClosestObject);
        }

        if (newObject != null)
        {
            AddOutline(newObject);
        }

        previousClosestObject = newObject;
    }

    void AddOutline(GameObject obj)
    {
        if (!obj.TryGetComponent(out Outline outline))
        {
            outline = obj.AddComponent<Outline>();
        }
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
    }

    void RemoveOutline(GameObject obj)
    {
        if (obj.TryGetComponent(out Outline outline))
        {
            Destroy(outline);
        }
    }

    // 디버그용으로 감지 범위 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
