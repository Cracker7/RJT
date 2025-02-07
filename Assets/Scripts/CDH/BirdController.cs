//using UnityEngine;

//public class ADkey1 : MonoBehaviour
//{
//    //�̵� �ӵ�
//    [SerializeField] private float MoveSpeed = 50f;
//    //�ε巯�� �̵��� ���� �ð�
//    [SerializeField] private float smoothTime = 0.1f;

//    public GameObject bird;

//    //��ǥ ��ġ
//    private Vector3 TargetPosition;
//    //���� �ӵ�
//    private Vector3 velocity = Vector3.zero;


//    private void Update()
//    {
//        ADObjection(bird);
//    }

//    public void ADObjection(GameObject _object)
//    {
//        TargetPosition = _object.transform.position;
//        GameObject Object = _object;

//        // AŰ�� ������ �������� �̵�
//        if (Input.GetKey(KeyCode.A))
//        {
//            TargetPosition += Vector3.right * MoveSpeed * Time.deltaTime;
//        }

//        // DŰ�� ������ ���������� �̵�
//        if (Input.GetKey(KeyCode.D))
//        {
//            TargetPosition += Vector3.left * MoveSpeed * Time.deltaTime;
//        }

//        // �ε巴�� �̵�
//        Object.transform.position = Vector3.SmoothDamp(Object.transform.position, TargetPosition, ref velocity, smoothTime);
//    }


//}

//using UnityEngine;

//public class ADkey1 : MonoBehaviour
//{
//    [SerializeField] private float moveSpeed = 10f;  // 전진 속도
//    [SerializeField] private float turnSpeed = 50f;  // 회전 속도
//    [SerializeField] private float smoothTime = 0.1f;  // 부드러운 회전
//    [SerializeField] private float fallForce = 10f;  // 추가 중력 힘

//    public GameObject bird;  // 새 (박스)
//    private Rigidbody rb;  // Rigidbody 추가

//    private float targetRotationY;  // 목표 회전값
//    private float currentVelocity;  // 현재 회전 속도

//    private void Start()
//    {
//        rb = bird.GetComponent<Rigidbody>();
//        rb.useGravity = true;  // 중력 활성화
//        rb.mass = 5f;  // 무게를 증가시켜 더 빨리 떨어지게 함
//    }

//    private void FixedUpdate()
//    {
//        rb.linearVelocity = bird.transform.forward * moveSpeed;  // 앞으로 계속 이동

//        // 🎯 더 빠르게 떨어지도록 추가 힘을 줌!
//        rb.AddForce(Vector3.down * fallForce, ForceMode.Acceleration);

//        // 🎮 좌우 회전
//        if (Input.GetKey(KeyCode.A))
//        {
//            targetRotationY -= turnSpeed * Time.deltaTime;
//        }

//        if (Input.GetKey(KeyCode.D))
//        {
//            targetRotationY += turnSpeed * Time.deltaTime;
//        }

//        // 부드러운 회전 적용
//        float newYRotation = Mathf.SmoothDampAngle(bird.transform.eulerAngles.y, targetRotationY, ref currentVelocity, smoothTime);
//        bird.transform.rotation = Quaternion.Euler(0, newYRotation, 0);
//    }



//using UnityEngine;

//public class BirdController : MonoBehaviour
//{
//    [SerializeField] private float moveSpeed = 10f;  // 앞으로 가는 속도
//    [SerializeField] private float turnSpeed = 50f;  // 회전 속도
//    [SerializeField] private float tiltAmount = 30f;  // 기울어지는 정도
//    [SerializeField] private float smoothTime = 0.1f;  // 부드러운 움직임
//    [SerializeField] private float sideMoveFactor = 2f;  // 좌우 이동 추가 힘

//    private Rigidbody rb;
//    private float targetTilt;  // 목표 기울기 (Z축 회전)
//    private float currentVelocity;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.useGravity = true;  // 중력 적용
//    }

//    private void FixedUpdate()
//    {
//        // 1️⃣ 기본적으로 앞으로 이동
//        rb.linearVelocity = transform.forward * moveSpeed;

//        // 2️⃣ 좌우 회전 (기울어지면서 방향 전환)
//        if (Input.GetKey(KeyCode.A))
//        {
//            rb.AddTorque(Vector3.up * -turnSpeed);  // 왼쪽 회전
//            targetTilt = tiltAmount;  // 왼쪽으로 기울이기
//            rb.linearVelocity += Vector3.left * sideMoveFactor * Time.deltaTime;  // 살짝 좌측 이동 추가
//        }
//        else if (Input.GetKey(KeyCode.D))
//        {
//            rb.AddTorque(Vector3.up * turnSpeed);  // 오른쪽 회전
//            targetTilt = -tiltAmount;  // 오른쪽으로 기울이기
//            rb.linearVelocity += Vector3.right * sideMoveFactor * Time.deltaTime;  // 살짝 우측 이동 추가
//        }
//        else
//        {
//            targetTilt = 0;  // 입력이 없으면 다시 정면으로
//        }

//        // 3️⃣ 새가 부드럽게 기울어지도록 보간 (Z축 회전)
//        float newZRotation = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetTilt, ref currentVelocity, smoothTime);
//        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
//    }
//}




using UnityEngine;

public class BirdController : MonoBehaviour,IMovement
{
    [SerializeField] private float moveSpeed = 10f;  // 앞으로 가는 속도
    [SerializeField] private float turnSpeed = 50f;  // 회전 속도
    [SerializeField] private float tiltAmount = 30f;  // 기울어지는 정도
    [SerializeField] private float smoothTime = 0.3f;  // 기울어지는 속도
    [SerializeField] private float sideMoveFactor = 5f;  // 좌우 이동 힘
    [SerializeField] private Rigidbody[] ragdollLimbs;  // 래그돌 팔(관절)들
    [SerializeField] private float ragdollFlapForce = 10f;  // 래그돌이 팔락거리는 힘

    private Rigidbody rb;
    private float targetTilt;
    private float currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;  // 중력 적용
    }

    //private void FixedUpdate()
    //{
    //    rb.linearVelocity = transform.forward * moveSpeed;

    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        rb.AddTorque(Vector3.up * -turnSpeed);
    //        targetTilt = tiltAmount;
    //        rb.linearVelocity += Vector3.left * sideMoveFactor * Time.deltaTime;
    //        FlapRagdoll(Vector3.left);  // 래그돌 흔들기 (왼쪽)
    //    }
    //    else if (Input.GetKey(KeyCode.D))
    //    {
    //        rb.AddTorque(Vector3.up * turnSpeed);
    //        targetTilt = -tiltAmount;
    //        rb.linearVelocity += Vector3.right * sideMoveFactor * Time.deltaTime;
    //        FlapRagdoll(Vector3.right);  // 래그돌 흔들기 (오른쪽)
    //    }
    //    else
    //    {
    //        targetTilt = 0;
    //    }

    //    float newZRotation = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetTilt, ref currentVelocity, smoothTime);
    //    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
    //}

    private void FlapRagdoll(Vector3 direction)
    {
        foreach (Rigidbody limb in ragdollLimbs)
        {
            Vector3 randomForce = direction* -1 * ragdollFlapForce * Random.Range(0.8f, 1.2f);
            limb.AddForce(randomForce, ForceMode.Impulse);  // 랜덤한 힘을 추가
        }
    }

    public void Move(Vector3 input)
    {
        Debug.Log(input);
        rb.linearVelocity = transform.forward * moveSpeed;

        if (input.x < 0)
        {
            rb.AddTorque(transform.up * -turnSpeed);
            targetTilt = tiltAmount;
            rb.linearVelocity += -transform.right * sideMoveFactor * Time.deltaTime;
            FlapRagdoll(-transform.right);  // 래그돌 흔들기 (왼쪽)
        }
        else if (input.x > 0)
        {
            rb.AddTorque(transform.up * turnSpeed);
            targetTilt = -tiltAmount;
            rb.linearVelocity += transform.right * sideMoveFactor * Time.deltaTime;
            FlapRagdoll(transform.right);  // 래그돌 흔들기 (오른쪽)
        }
        else
        {
            targetTilt = 0;
        }

        float newZRotation = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetTilt, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
    }
}






