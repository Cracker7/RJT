using System.Linq;
using UnityEngine;

public class DrawOutline : MonoBehaviour
{
    public float moveSpeed = 5f;  // 이동 속도
    public float detectionRadius = 5f; // 감지 범위
    public LayerMask layer;

    private GameObject closestObject;
    private GameObject previousClosestObject;

    void FixedUpdate()
    {
        //MovePlayer();
        DetectClosestObject();
    }

    //void MovePlayer()
    //{
    //    float moveX = Input.GetAxisRaw("Horizontal");  // A, D (왼쪽, 오른쪽)
    //    float moveZ = Input.GetAxisRaw("Vertical");    // W, S (위, 아래)

    //    Vector3 move = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed * Time.deltaTime;
    //    if (move != Vector3.zero)
    //    {
    //        transform.Translate(move, Space.World);
    //    }
    //}

    //void DetectClosestObject()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, layer);

    //    GameObject newClosestObject = colliders
    //        .Select(c => c.gameObject)
    //        .OrderBy(go => (go.transform.position - transform.position).sqrMagnitude)
    //        .FirstOrDefault();

    //    if (newClosestObject != closestObject)
    //    {
    //        UpdateOutlineEffect(newClosestObject);
    //        closestObject = newClosestObject;
    //    }
    //}

    public void DetectClosestObject()
    {
        // Physics.OverlapSphere로 주변의 콜라이더들을 찾은 후,
        // 1. 플레이어 앞쪽에 있는지 (Dot product >= 0) 체크
        // 2. InteractableObject 컴포넌트가 존재하는지
        // 3. currentInteractableObject 혹은 currentObjectPrefab과 중복되지 않는지 체크한 뒤
        // 4. 가장 가까운 순서로 정렬해 첫 번째 요소를 선택합니다.
        GameObject newClosestObject = Physics.OverlapSphere(transform.position, detectionRadius, layer)
            .Where(c => Vector3.Dot(transform.forward, (c.transform.position - transform.position).normalized) >= 0)
            .Select(c => c.gameObject)
            .OrderBy(io => (io.transform.position - transform.position).sqrMagnitude)
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

        outline.enabled = true;
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.red;
        outline.OutlineWidth = 15f;
    }

    void RemoveOutline(GameObject obj)
    {
        if (obj.TryGetComponent(out Outline outline))
        {
            //Destroy(outline);
            outline.enabled = false;
        }
    }

    // 디버그용으로 감지 범위 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
