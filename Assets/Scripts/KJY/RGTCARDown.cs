using UnityEngine;
using System.Collections;

public class RGTCARDown : MonoBehaviour
{
    public float sinkSpeed = 0.5f; // 땅에 빨려 들어가는 속도
    public float riseSpeed = 1.0f; // 올라가는 속도
    public float maxSinkDepth = 2.0f; // 최대 빨려 들어가는 깊이
    public float moveThreshold = 0.5f; // 움직일 수 있는 깊이 임계값 (절반 이상)

    private float currentDepth = 0f; // 현재 깊이
    private bool isRising = false; // 올라가는 중인지 여부
    private Vector3 originalPosition; // 원래 위치

    void Start()
    {
        originalPosition = transform.position; // 원래 위치 저장
    }

    void Update()
    {
        // 탈것의 속도가 60 아래인지 확인
        if (GetVehicleSpeed() < 60f)
        {
            if (!isRising) // 올라가는 중이 아니면
            {
                SinkIntoQuicksand(); // 땅에 빨려 들어감
            }
        }

        // 연타 입력 감지 (Spacebar를 연타로 가정)
        if (Input.GetKey(KeyCode.Z))
        {
            StartRising(); // 올라가기 시작
        }

        if (isRising)
        {
            RiseFromQuicksand(); // 올라가는 로직 실행
        }
    }

    // 탈것의 속도를 가져오는 함수 (임시 구현)
    float GetVehicleSpeed()
    {
        // 여기에 탈것의 속도를 계산하는 로직을 추가하세요.
        // 예: Rigidbody의 velocity.magnitude를 사용
        return GetComponent<Rigidbody>().linearVelocity.magnitude;
    }

    // 땅에 빨려 들어가는 로직
    void SinkIntoQuicksand()
    {
        if (currentDepth < maxSinkDepth)
        {
            currentDepth += sinkSpeed * Time.deltaTime; // 깊이 증가
            transform.position = originalPosition - Vector3.up * currentDepth; // 위치 업데이트
        }
    }

    // 올라가기 시작
    void StartRising()
    {
        isRising = true; // 올라가는 상태로 전환
    }

    // 땅에서 올라가는 로직
    void RiseFromQuicksand()
    {
        if (currentDepth > 0f)
        {
            currentDepth -= riseSpeed * Time.deltaTime; // 깊이 감소
            transform.position = originalPosition - Vector3.up * currentDepth; // 위치 업데이트
        }
        else
        {
            isRising = false; // 올라가는 상태 종료
        }
    }

    // 움직일 수 있는지 여부 확인
    bool CanMove()
    {
        return currentDepth < maxSinkDepth * moveThreshold; // 절반 이상 땅 위에 있으면 true
    }

    // 탈것 이동 로직 (예시)
    void MoveVehicle()
    {
        if (CanMove())
        {
            // 여기에 탈것 이동 로직을 추가하세요.
            // 예: Rigidbody.AddForce 또는 transform.Translate 사용
        }
    }

}
