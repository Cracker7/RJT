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
    
    // HP 업데이트를 위한 델리게이트 선언
    public delegate void OnHPUpdateDelegate();
    public OnHPUpdateDelegate onHPUpdate;

    public event Action OnDestroyCalled;
    public event Action OnHpBarTr;

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

    private void Update()
    {
        if(onHPUpdate != null && OnDestroyCalled != null)
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
        StartCoroutine(DecreaseHpCoroutine());
    }

    // 1초 동안 5만큼 HP를 부드럽게 감소시키는 코루틴
    private IEnumerator DecreaseHpCoroutine()
    {
        // // 감소할 양 및 코루틴 지속 시간 (1초)
        // float decreaseAmount = 5f;
        // float duration = 1f;
        // float elapsed = 0f;
        
        // // 시작 HP와 최종 HP 값 계산
        // float startHP = currentDurability;
        // float targetHP = Mathf.Max(currentDurability - decreaseAmount, 0f);

        // // 1초 동안 매 프레임마다 선형 보간(Lerp)로 HP 감소
        // while (elapsed < duration)
        // {
        //     elapsed += Time.deltaTime;
        //     currentDurability = Mathf.Lerp(startHP, targetHP, elapsed / duration);
        //     hpBar.UpdateHpBar(currentDurability, maxDurability);
        //     yield return null;
        // }

        // // 코루틴 종료 시 확실히 targetHP로 설정
        // currentDurability = targetHP;
        while(currentDurability > 0){
        Debug.Log("체력 함수 실행됨");

        currentDurability -= 1f;

        Debug.Log("현재 체력" + currentDurability);

        hpBar.UpdateHpBar(maxDurability, currentDurability);

        yield return new WaitForSeconds(1f);
        }
        
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

    private void OnDestroy()
    {
        // 델리게이트 정리
        onHPUpdate = null;
    }
}