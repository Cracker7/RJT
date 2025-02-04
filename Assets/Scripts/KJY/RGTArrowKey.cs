using UnityEngine;

public class RGTArrowKey : MonoBehaviour, IMovement
{

    //이동속도
    [SerializeField] private float moveSpeed = 50f; 
    //부드러운 이동시간
    [SerializeField] private float smoothTime = 0.1f; 

    //목표위치
    private Vector3 targetPosition; 
    //현재속도
    private Vector3 velocity = Vector3.zero;


    //public void ArrowKey(GameObject _object)
    //{
    //    GameObject Object = _object;
    //    targetPosition = Object.transform.position; 
    //    // 방향키 입력 감지 (좌우 이동)
    //    float moveInput = Input.GetAxis("Horizontal"); // 왼쪽(-1), 오른쪽(1)
    //    targetPosition += Vector3.right * moveInput * moveSpeed * Time.deltaTime;

    //    // 부드럽게 이동
    //    Object.transform.position = Vector3.SmoothDamp(Object.transform.position, targetPosition, ref velocity, smoothTime);
    //}

    public void Move(float input)
    {
        Vector3 currentPosition = transform.position;
        //Vector3 targetPosition = currentPosition + input * moveSpeed * Time.deltaTime;

        // SmoothDamp를 이용해 부드럽게 목표 위치로 이동
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothTime);
    }
}
