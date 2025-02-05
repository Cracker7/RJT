using UnityEngine;

public class mocma : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 10f;  // 앞으로 가는 속도
    [SerializeField] private float turnSpeed = 50f;  // 회전 속도
    //[SerializeField] private float tiltAmount = 30f;  // 기울어지는 정도
    //[SerializeField] private float smoothTime = 0.3f;  // 기울어지는 속도
    [SerializeField] private float sideMoveFactor = 5f;  // 좌우 이동 힘
    [SerializeField] private Rigidbody[] ragdollLimbs;  // 래그돌 팔(관절)들
    [SerializeField] private float ragdollFlapForce = 10f;  // 래그돌이 팔락거리는 힘

    public Rigidbody rb;
    private float targetTilt;
    private float currentVelocity;

    //private void Start()
    //{
    //    //rb = GetComponent<Rigidbody>();
    //    rb.useGravity = true;  // 중력 적용
    //}

    private void FixedUpdate()
    {
        //rb.linearVelocity = transform.forward * moveSpeed;

            FlapRagdoll(Vector3.back);  // 래그돌 흔들기 (왼쪽)

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(Vector3.up * turnSpeed);
            //targetTilt = tiltAmount;
            rb.linearVelocity += Vector3.right * sideMoveFactor * Time.deltaTime;
            FlapRagdoll(Vector3.left);  // 래그돌 흔들기 (왼쪽)
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(Vector3.up * -turnSpeed);
           // targetTilt = -tiltAmount;
            rb.linearVelocity += Vector3.left * sideMoveFactor * Time.deltaTime;
            FlapRagdoll(Vector3.right);  // 래그돌 흔들기 (오른쪽)
        }
        //else
        //{
        //    targetTilt = 0;
        //}

        //float newZRotation = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetTilt, ref currentVelocity, smoothTime);
       // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
    }

    private void FlapRagdoll(Vector3 direction)
    {
        foreach (Rigidbody limb in ragdollLimbs)
        {
            Vector3 randomForce = direction * -1 * ragdollFlapForce * Random.Range(0.8f, 1.2f);
            limb.AddForce(randomForce, ForceMode.Impulse);  // 랜덤한 힘을 추가
        }
    }
}






