using UnityEngine;

public class RGTCARDown : MonoBehaviour
{
    public float sinkDepth = 2f;        // 최대 가라앉는 깊이
    public float sinkSpeed = 0.5f;      // 가라앉는 속도
    public float riseSpeed = 0.3f;      // 떠오르는 속도
    public float requiredTapSpeed = 0.2f; // 연타 감지 시간
    public float speedThreshold = 60f;  // 속도 임계값 (60 이하일 때 가라앉음)

    private Vector3 startPos;  // 원래 위치 저장
    private Vector3 targetPos; // 목표 위치
    private bool isSinking = false;
    private bool isRising = false;
    private float lastTapTime = 0f;
    private Rigidbody rb; // 차량 물리 제어

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        targetPos = startPos;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Quicksand"))
        {
            float currentSpeed = rb.linearVelocity.magnitude * 3.6f; // 속도를 km/h로 변환

            if (currentSpeed < speedThreshold && !isSinking)
            {
                StartSinking();
            }
        }
    }

    void Update()
    {
        if (isSinking)
        {
            if (isRising)
            {
                targetPos += new Vector3(0, riseSpeed * Time.deltaTime, 0);
                targetPos.y = Mathf.Min(targetPos.y, startPos.y); // 원래 위치 이상 올라가지 않도록 제한
            }
            else
            {
                targetPos -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
                targetPos.y = Mathf.Max(targetPos.y, startPos.y - sinkDepth); // 최대 깊이 이하로 못 가게 제한
            }

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
        }

        // 🔥 연타 감지 (Space, W 키)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastTapTime < requiredTapSpeed) // 연타 시 상승
            {
                isRising = true;
            }
            lastTapTime = Time.time;
        }

        // 🌟 차량의 절반 이상이 땅 위에 있으면 이동 가능
        if (transform.position.y >= startPos.y - (sinkDepth / 2))
        {
            rb.linearDamping = 0.1f; // 이동 가능 (마찰 감소)
        }
        else
        {
            rb.linearDamping = 5f; // 이동 불가능 (마찰 증가)
        }
    }

    void StartSinking()
    {
        isSinking = true;
        targetPos = transform.position;
    }
}
