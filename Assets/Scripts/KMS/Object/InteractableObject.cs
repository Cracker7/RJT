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

            Rigidbody rb = other.GetComponentInParent<Rigidbody>();

            Debug.Log("트리거된 물체" + rb);
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                // 수평 방향으로 -1 ~ 1 사이의 랜덤 값 생성 (수평 방향만 사용)
                Vector3 randomHorizontal = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(0.3f, 1f)).normalized;

                // 기본적으로 위쪽으로 날아가게 하고, 수평 방향의 랜덤 요소를 약간 추가
                // upwardForce가 클수록 위쪽 성분이 강해집니다.
                Vector3 bounceDirection = (Vector3.up * upwardForce + randomHorizontal).normalized;

                // impulse 방식으로 힘과 토크 적용
                rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
                rb.AddTorque(UnityEngine.Random.insideUnitSphere * bounceForce/10, ForceMode.Impulse);
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

    private void OnDestroy()
    {
        // 델리게이트 정리
        onRideUpdate = null;
    }
}