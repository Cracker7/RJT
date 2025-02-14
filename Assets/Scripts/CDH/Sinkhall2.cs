using UnityEngine;

public class Sinkhall2 : MonoBehaviour
{
    public Rigidbody rb;  // 자전거의 리지드바디
    public float sinkSpeed = 1f;  // 가라앉는 속도
    public float maxSinkDepth = 3f;  // 최대 가라앉을 깊이
    public float riseSpeed = 2f;  // 올라오는 속도
    public int keyPressThreshold = 5;  // 왼쪽/오른쪽 키 연타 횟수
    private int keyPressCount = 0;  // 방향키 연타 카운트
    private bool isSinking = false;  // 현재 가라앉고 있는지 여부
    private bool isRising = false;  // 현재 올라가고 있는지 여부
    private Vector3 originalPosition;  // 원래 위치

    private void Start()
    {
        originalPosition = transform.position;  // 원래 위치 저장
    }

    private void Update()
    {
        // 자전거 속도 체크 (60 이하일 때 가라앉기 시작)
        if (rb.linearVelocity.magnitude < 60f && !isRising)
        {
            isSinking = true;
        }

        if (isSinking)
        {
            // 가라앉기 (Rigidbody를 사용하여 자연스럽게 가라앉도록 수정)
            float targetY = originalPosition.y - maxSinkDepth;
            float step = sinkSpeed * Time.deltaTime;
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            // Rigidbody로 위치 변경
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, step));

            // 최대 가라앉기 깊이에 도달하면 연타 시작
            if (transform.position.y <= targetY)
            {
                isSinking = false;  // 가라앉기가 끝나면 연타 대기
            }
        }

        // 방향키 연타 감지
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            keyPressCount++;
            if (keyPressCount >= keyPressThreshold && !isRising)
            {
                isRising = true;
            }
        }

        // 올라오기
        if (isRising)
        {
            float targetY = originalPosition.y;
            float step = riseSpeed * Time.deltaTime;
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            // Rigidbody로 위치 변경
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, step));

            if (transform.position.y >= targetY)
            {
                isRising = false;
                // 완전히 올라오면 살짝 튕기기 (위로 튕기기 효과)
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            }
        }
    }

    // 가라앉기 중 속도에 따라 점점 내려가게 만들 수 있는 코드 (중력 적용)
    private void FixedUpdate()
    {
        if (isSinking)
        {
            // 중력을 추가로 적용하여 더 빠르게 내려가도록 할 수 있습니다.
            rb.AddForce(Vector3.down * sinkSpeed, ForceMode.Acceleration);
        }
    }
}
