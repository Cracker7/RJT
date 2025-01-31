using UnityEngine;

public class RGTCharacterBalanceController : MonoBehaviour
{
    public Transform rollingObject; // 굴러가는 물체의 Transform
    public Rigidbody characterRigidbody; // 캐릭터의 Rigidbody
    public float followSpeed = 5f; // 물체를 따라가는 기본 이동 속도
    public float balanceSpeed = 2f; // 균형 조절 속도
    public float maxBalanceDistance = 4f; // 균형을 유지할 수 있는 최대 거리

    public float randomMoveSpeed = 0.1f; // 랜덤 이동 속도
    public float randomMoveInterval = 2f; // 랜덤 이동 간격

    private Vector3 offset; // 물체와 캐릭터 간 초기 위치 차이
    private Vector3 randomDirection; // 랜덤 이동 방향
    private float nextRandomMoveTime; // 다음 랜덤 이동 시간


    void Start()
    {
        if (rollingObject == null || characterRigidbody == null)
        {
            Debug.LogError("RollingObject 또는 CharacterRigidbody가 설정되지 않았습니다.");
            return;
        }

        // 초기 위치 차이 계산
        offset = transform.position - rollingObject.position;

        // 캐릭터의 회전을 고정
        characterRigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (rollingObject == null) return;

        // 물체의 현재 위치 + 초기 오프셋 계산
        Vector3 targetPosition = rollingObject.position + offset;

        // 방향키 입력으로 균형 조정
        Vector3 balanceAdjustment = Vector3.zero;

        if (balanceAdjustment == Vector3.zero)
        {
            // 일정 시간마다 랜덤 방향 갱신
            if (Time.time >= nextRandomMoveTime)
            {
                SetRandomDirection();
            }
            balanceAdjustment += randomDirection * randomMoveSpeed * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) // ← 키
            balanceAdjustment += Vector3.left * balanceSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) // → 키
            balanceAdjustment += Vector3.right * balanceSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.UpArrow)) // ↑ 키
            balanceAdjustment += Vector3.forward * balanceSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.DownArrow)) // ↓ 키
            balanceAdjustment += Vector3.back * balanceSpeed * Time.fixedDeltaTime;

        // 균형 조정을 적용한 목표 위치
        targetPosition += balanceAdjustment;

        // 캐릭터 이동
        characterRigidbody.MovePosition(targetPosition);

        // 균형 여부 확인
        CheckBalance(targetPosition);

        Debug.Log("Pos : " + transform.position);
    }

    private void CheckBalance(Vector3 targetPosition)
    {
        // 캐릭터가 물체 중심에서 너무 멀어지면 균형을 잃었다고 판단
        float distanceFromCenter = Vector3.Distance(rollingObject.position, transform.position);

        if (distanceFromCenter > maxBalanceDistance)
        {
            // 균형을 잃었을 때 처리
            Debug.Log("균형을 잃었습니다!");
            characterRigidbody.useGravity = true; // 중력을 활성화하여 캐릭터가 떨어지도록 함
            this.enabled = false; // 스크립트를 비활성화하여 더 이상 따라가지 않음
        }
    }

    private void SetRandomDirection()
    {
        // 랜덤한 방향 설정
        randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized;

        // 다음 랜덤 이동 시간 설정
        nextRandomMoveTime = Time.time + randomMoveInterval;
    }

}