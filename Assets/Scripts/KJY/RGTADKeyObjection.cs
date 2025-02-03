using UnityEngine;

public class RGTADKeyObjection : MonoBehaviour, IMovement
{
    //이동 속도
    [SerializeField] private float moveSpeed = 50f;
    //부드러운 이동을 위한 시간
    [SerializeField] private float smoothTime = 0.1f;

    //목표 위치
    private Vector3 TargetPosition;
    //현재 속도
    private Vector3 velocity = Vector3.zero;


    //public void ADObjection(GameObject _object)
    //{
    //    TargetPosition = _object.transform.position;
    //    GameObject Object = _object;

    //    // A키를 누르면 왼쪽으로 이동
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        TargetPosition += Vector3.right * MoveSpeed * Time.deltaTime;
    //    }

    //    // D키를 누르면 오른쪽으로 이동
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        TargetPosition += Vector3.left * MoveSpeed * Time.deltaTime;
    //    }

    //    // 부드럽게 이동
    //    Object.transform.position = Vector3.SmoothDamp(Object.transform.position, TargetPosition, ref velocity, smoothTime);
    //}

    public void Move(Vector3 input)
    {
        // 현재 위치를 기준으로 targetPosition을 계산합니다.
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = currentPosition + input * moveSpeed * Time.deltaTime;

        // SmoothDamp를 이용해 부드럽게 targetPosition으로 이동합니다.
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocity, smoothTime);
    }
}
