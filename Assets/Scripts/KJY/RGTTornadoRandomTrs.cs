using System;
using UnityEngine;


public class RGTTornadoRandomTrs : MonoBehaviour
{
    [SerializeField] private Transform[] positions; // 사용할 위치 배열
    [SerializeField] private GameObject obj; // 이동할 오브젝트
    [SerializeField] private float speed = 10f; // 이동 속도
    [SerializeField] Transform originPos;


    private Vector3 Opos;
    private int currentIndex = 0; // 현재 목표 위치 인덱스
    private Vector3 targetPosition; // 목표 위치

    private void Start()
    {
        // 초기 목표 위치 설정
        SetNextTargetPosition();
    }

    private void Update()
    {
        MoveToNextPosition();
        //Ray ray = new Ray(transform.position, transform.forward);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, palyer))
        //{
        //    hit.collider.gameObject.transform.position = originPos.position;
        //}
    }

    // 목표 위치로 이동하는 함수
    private void MoveToNextPosition()
    {
        // 부드럽게 목표 위치로 이동
        obj.transform.position = Vector3.MoveTowards(
            obj.transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // 목표 위치에 도달하면 다음 위치 설정
        if (Vector3.Distance(obj.transform.position, targetPosition) < 0.1f)
        {
            SetNextTargetPosition();
        }
    }

    // 다음 목표 위치 설정 함수
    private void SetNextTargetPosition()
    {
        currentIndex = (currentIndex + 1) % positions.Length;
        targetPosition = positions[currentIndex].position;
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            other.gameObject.transform.position = originPos.position;
        }
    }
}