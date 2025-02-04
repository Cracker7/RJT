using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 10f;  // 마우스 이동 속도
    public float smoothTime = 0.2f; // 부드러운 이동

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 클릭 중
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero); // 평면 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPos = ray.GetPoint(distance);
                Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

                rb.MovePosition(smoothedPos); // 힘을 이용한 이동
            }
        }
    }
}
