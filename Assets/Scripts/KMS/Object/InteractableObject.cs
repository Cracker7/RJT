using UnityEngine;
using System.Collections;
using System;

public class InteractableObject : MonoBehaviour
{
    public float maxDurability;
    public float currentDurability;

    public Transform mountPoint;
    public ObjectSpecificData objectData;

    public IMovement movementController;
    public IInputHandler inputHandler;
    public RGTHpBar hpBar;
    private float damagePerSecond = 1f;
    public bool timeDamage = false;

    private bool collisionTriggered = false;      // 충돌이 발생했는지 여부 플래그
    private float collisionCooldown = 0.4f;           // 충돌 쿨다운 시간 (초 단위)
    private Coroutine collisionCooldownCoroutine = null; // 실행중인 코루틴 참조

    // HP 업데이트를 위한 델리게이트 선언
    public delegate void OnRideUpdateDelegate();
    public OnRideUpdateDelegate onRideUpdate;

    public delegate void OnRideColUpdateDelegate();
    public OnRideColUpdateDelegate onRideCol;

    public event Action OnDestroyCalled;
    public event Action OnHpBarTr;

    Rigidbody rb;

    private float bounceForce = 100f;  // 날아가는 힘의 크기
    private float upwardForce = 10f;   // 위로 뜨는 힘의 크기

    public virtual void Awake()
    {
        Init();
        currentDurability = objectData.durability;
        maxDurability = objectData.durability;

        if (hpBar != null)
        {
            hpBar.UpdateHpBar(maxDurability, maxDurability);
        }
    }

    private void OnEnable()
    {
        collisionCooldownCoroutine = StartCoroutine(CollisionCooldownCoroutine());
    }

    //private void Start()
    //{
    //    hpBar.UpdateHpBar(maxDurability, maxDurability);

    //}

    private void Update()
    {
        if(onRideUpdate != null && OnDestroyCalled != null)
        {
            UpdateHpBarTr();
        }
    }

    public void Init()
    {
        movementController = GetComponent<IMovement>();
        inputHandler = GetComponent<IInputHandler>();
        hpBar = FindFirstObjectByType<RGTHpBar>();
        if(mountPoint == null)
        {
            mountPoint = transform;
        }
    }

    // 외부에서 호출하면 코루틴을 통해 HP를 부드럽게 감소시킵니다.
    public void StartHpDecrease()
    {
        // Debug.Log("체력 함수 실행됨");
        // currentDurability -= 1f;
        // hpBar.UpdateHpBar(currentDurability, maxDurability);
        if (!timeDamage) return;
        StartCoroutine(DecreaseHpCoroutine());
    }

    // 1초 동안 5만큼 HP를 부드럽게 감소시키는 코루틴
    private IEnumerator DecreaseHpCoroutine()
    {
        while(currentDurability > 0){
            Debug.Log("체력 함수 실행됨");

            currentDurability -= damagePerSecond * Time.deltaTime;

            Debug.Log("현재 체력" + currentDurability);

            hpBar.UpdateHpBar(maxDurability, currentDurability);

            yield return null;
        }
        
        if (currentDurability <= 0)
        {
            DestroyObject();
        }
    }

    // 충돌이나 트리거 발생 시 호출될 onRideUpdate에 등록된 함수입니다.
    public void HandleCollisionDamage()
    {
        currentDurability -= maxDurability/10;
        Debug.Log("충돌/트리거로 인한 체력 감소. 남은 체력: " + currentDurability);

        hpBar.UpdateHpBar(maxDurability, currentDurability);

        if (currentDurability <= 0)
        {
            DestroyObject();
        }
    }


    // hp바 위치를 업데이트 하는 함수
    public void UpdateHpBarTr()
    {   
        hpBar.UpdatePosition(transform);

        // 이벤트에 등록되어있는게 있다면 실행
        OnHpBarTr?.Invoke();
    }

    // 체력이 0이 되면 파괴되는 함수 실행 후 오브젝트 삭제
    public void DestroyObject()
    {
        Debug.Log("오브젝트 파괴됨");

        OnDestroyCalled?.Invoke();

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("장애물에 부딪힘");
        if (!collisionTriggered /*&& collision.gameObject.CompareTag("Obstacle")*/)
        {
            onRideCol?.Invoke();
            collisionCooldownCoroutine = StartCoroutine(CollisionCooldownCoroutine());
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("발판을 밟음");
        if (!collisionTriggered /* && other.CompareTag("Platform") */)
        {
            Debug.Log("Trigger 발판 밟음");
            onRideCol?.Invoke();
            collisionCooldownCoroutine = StartCoroutine(CollisionCooldownCoroutine());

            rb = transform.GetComponent<Rigidbody>();

            // 충돌 지점을 판정합니다.
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            // 차량 위치에서 충돌 지점으로 향하는 방향 벡터 (정규화)
            Vector3 collisionDirection = (collisionPoint - transform.position).normalized;

            // 차량의 진행 방향 (또는 속도 방향)을 얻습니다.
            // 일반적으로 차량의 transform.forward가 진행 방향입니다.
            //Vector3 vehicleForward = transform.forward;
            // 혹은 속도 방향으로 판단하고 싶다면 아래처럼 할 수 있습니다.
             Vector3 vehicleVelocityDirection = rb.linearVelocity.normalized;

            // 차량 진행 방향과 충돌 방향 사이의 각도를 구합니다.
            float angle = Vector3.Angle(vehicleVelocityDirection, collisionDirection);
            Debug.Log("차량 진행 방향과 충돌 방향 사이의 각도: " + angle);

            // 45도 이하이면 정면 충돌, 그 이상이면 측면 충돌로 판단합니다.
            if (angle < 45f)
            {
                Debug.Log("EEE 정면 충돌");
                // 정면 충돌이면 충돌된 곳을 기준으로 반대 방향으로 튕기게 함
                //StartCoroutine(BounceBackCoroutine(collisionPoint));
            }
            else
            {
                Debug.Log("EEE 측면 충돌");

                // 이름이 "Mesh"인 게임 오브젝트를 찾습니다.
                Transform meshTransform = transform.Find("Mesh");
                if (meshTransform == null)
                {
                    // 직접적인 자식으로 찾지 못한 경우, 전체 하위 계층에서 검색합니다.
                    foreach (Transform child in transform.GetComponentsInChildren<Transform>())
                    {
                        if (child.name.Equals("Mesh"))
                        {
                            meshTransform = child;
                            break;
                        }
                    }
                }

                if (meshTransform != null)
                {
                    Debug.Log("Mesh 오브젝트 찾음: " + meshTransform.gameObject.name);
                    // 예시: 전역 기준으로 Y축을 중심으로 90도 회전시킵니다.
                    StartCoroutine(RotateCoroutine(meshTransform));
                }
                else
                {
                    Debug.Log("Mesh 이름의 자식 게임 오브젝트를 찾지 못함");
                }
            }

            // 현재 차량의 속도 벡터도 출력 (디버깅용)
            Vector3 currentVelocity = rb.linearVelocity;
            Debug.Log("현재 속도 벡터: " + currentVelocity);

            // 기존 로직: 충돌한 물체의 Rigidbody를 가져와서 물리 효과 적용
            Rigidbody rbOther = other.GetComponentInParent<Rigidbody>();
            Debug.Log("트리거된 물체: " + rbOther);
            if (rbOther != null)
            {
                rbOther.isKinematic = false;
                rbOther.useGravity = true;

                // 수평 방향으로 -1 ~ 1 사이의 랜덤 값 생성 (수평 성분만 사용)
                Vector3 randomHorizontal = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(0.3f, 1f)).normalized;

                // 기본적으로 위쪽으로 날아가게 하면서 수평 방향 랜덤 요소를 추가
                Vector3 bounceDirection = (Vector3.up * upwardForce + randomHorizontal).normalized;

                // impulse 방식으로 힘 적용
                rbOther.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
                // 필요 시 토크도 적용할 수 있습니다.
                // rbOther.AddTorque(UnityEngine.Random.insideUnitSphere * (bounceForce / 10f), ForceMode.Impulse);
            }
        }
    }

    private IEnumerator CollisionCooldownCoroutine()
    {
        collisionTriggered = true;
        yield return new WaitForSeconds(collisionCooldown);
        collisionTriggered = false;
        collisionCooldownCoroutine = null;
    }

    private IEnumerator RotateCoroutine(Transform meshTransform)
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = meshTransform.rotation; // 초기 회전값 저장

        if (rb != null)
        {
            rb.linearVelocity *= 0.7f;
        }

        while (elapsedTime < 1f)
        {
            // 매 프레임마다 Y축 기준으로 2000 * Time.deltaTime 만큼 회전
            meshTransform.Rotate(Vector3.up, 2000f * Time.deltaTime);

            // 경과 시간 누적
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전이 끝난 후 원래의 회전 상태(0,0,0)로 되돌리기
        meshTransform.rotation = initialRotation;
    }

    private void OnDestroy()
    {
        // 델리게이트 정리
        onRideUpdate = null;
    }
}