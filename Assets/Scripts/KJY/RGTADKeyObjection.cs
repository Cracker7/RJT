using UnityEngine;

public class RGTADKeyObjection : MonoBehaviour
{
    //이동 속도
    [SerializeField] private float MoveSpeed = 50f;
    //부드러운 이동을 위한 시간
    [SerializeField] private float smoothTime = 0.1f;

    //목표 위치
    private Vector3 TargetPosition;
    //현재 속도
    private Vector3 velocity = Vector3.zero;


    public void ADObjection(GameObject _object)
    {
        TargetPosition = _object.transform.position;
        GameObject Object = _object;

        // A키를 누르면 왼쪽으로 이동
        if (Input.GetKey(KeyCode.A))
        {
            TargetPosition += Vector3.right * MoveSpeed * Time.deltaTime;
        }

        // D키를 누르면 오른쪽으로 이동
        if (Input.GetKey(KeyCode.D))
        {
            TargetPosition += Vector3.left * MoveSpeed * Time.deltaTime;
        }

        // 부드럽게 이동
        Object.transform.position = Vector3.SmoothDamp(Object.transform.position, TargetPosition, ref velocity, smoothTime);
    }

}
