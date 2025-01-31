using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGTCapsule : MonoBehaviour
{

    public Transform rollingObject; // 굴러가는 물체의 Transform
    public Rigidbody characterRigidbody; // 캐릭터의 Rigidbody

    private Vector3 offset; // 캐릭터와 물체 사이의 초기 위치 차이

    void Start()
    {
        if (rollingObject == null || characterRigidbody == null)
        {
            Debug.LogError("RollingObject 또는 CharacterRigidbody가 설정되지 않았습니다.");
            return;
        }

        // 캐릭터와 물체 간 초기 위치 차이를 계산
        offset = transform.position - rollingObject.position;

        // 캐릭터의 회전을 고정하여 물체의 회전에 영향을 받지 않도록 설정
        characterRigidbody.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (rollingObject == null) return;

        // 물체의 현재 위치 + 초기 오프셋을 목표 위치로 계산
        Vector3 targetPosition = rollingObject.position + offset;

        // 캐릭터를 목표 위치로 부드럽게 이동
        characterRigidbody.MovePosition(targetPosition);
    }


}
