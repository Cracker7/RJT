using System.Collections.Generic;
using UnityEngine;

public class PlayerKMS : MonoBehaviour
{
    // 상태 머신
    private enum PlayerState { Idle, Transitioning, Riding, Dead }
    private PlayerState currentState = PlayerState.Idle;

    // 컴포넌트 캐싱
    private List<SkinnedMeshRenderer> skinRenderer;
    public IInputHandler currentInput;
    public IMovement currentMovement;

    // 래그돌 물리 컴포넌트 캐싱
    private List<Rigidbody> ragdollRigidbodies;
    private List<Collider> ragdollColliders;
    private Rigidbody mainRigidbody;  // 루트 오브젝트의 리지드바디
    private Collider mainCollider;     // 루트 오브젝트의 콜라이더

    // 물리 설정
    [Header("물리 설정")]
    public bool useGravity = true;
    public bool isKinematic = false;

    // 상호 작용 관련
    private InteractableObject currentInteractableObject;
    [Header("상호작용 설정")]
    public float interactionRange = 2f;
    public KeyCode interactKeyCode = KeyCode.G;
    public float maxRange = 10f;
    public float minRange = 2f;

    // 포물선 이동 관련
    [Header("포물선 이동 설정")]
    public float transitionDuration = 1f;
    public float jumpHeight = 3f;
    public float mountThreshold = 0.5f;
    private Vector3 startPosition;
    private float transitionTime = 0f;
    private InteractableObject targetObject = null;
    private Vector3 lastKnownMountPoint;

    [Header("시간 설정")]
    public float timeScale = 0.01f;
    private bool hasSlowedTime = false;

    [Space(10)]
    public sliderM miniGame;
    public GameObject currentObjectPrefab;

    // 속도 전달 관련
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;
    private bool hasSavedVelocity = false;

    private void Awake()
    {
        // 기본 컴포넌트 캐싱
        skinRenderer = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
        currentMovement = GetComponent<IMovement>();
        currentInput = GetComponent<IInputHandler>();

        // 래그돌 컴포넌트 캐싱
        CacheRagdollComponents();

        // 초기 물리 상태 설정
        SetPhysicsState(true, false, false);
    }

    // 래그돌 컴포넌트들을 캐시하는 메서드
    private void CacheRagdollComponents()
    {
        // 메인 컴포넌트 캐시
        mainRigidbody = GetComponent<Rigidbody>();
        mainCollider = GetComponent<Collider>();

        // 자식 컴포넌트들 캐시
        ragdollRigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        ragdollColliders = new List<Collider>(GetComponentsInChildren<Collider>());

        // 메인 컴포넌트가 리스트에 포함되어 있다면 제거
        if (mainRigidbody != null && ragdollRigidbodies.Contains(mainRigidbody))
        {
            ragdollRigidbodies.Remove(mainRigidbody);
        }
        if (mainCollider != null && ragdollColliders.Contains(mainCollider))
        {
            ragdollColliders.Remove(mainCollider);
        }
    }

    private void OnEnable()
    {
        //sliderM.OnShutdown += ResetTimeScale;
    }

    private void OnDisable()
    {
        //sliderM.OnShutdown -= ResetTimeScale;
    }

    private void Update()
    {
        // Transitioning 상태일 때는 입력을 무시한다.
        if (currentState == PlayerState.Transitioning)
        {
            UpdateTransition();
        }
        else if (currentState == PlayerState.Dead)
        {
            // 플레이어가 죽었을 때 실행하는 함수

            // 부모 관계 해제
            if (currentObjectPrefab != null)
                transform.SetParent(null);

            // 메시 렌더러 활성화
            foreach (SkinnedMeshRenderer skin in skinRenderer)
            {
                skin.enabled = true;
            }
            
            // 죽었을 때의 추가 로직 (미니게임 실패, 내구도 소진 등)
        }
        else
        {
            // 이전에는 Idle이나 Riding 상태에서 상호작용 키 입력을 처리했으나,
            // 이제 탑승은 충돌 시(OnCollisionEnter) 처리하므로 입력 관련 코드는 제거합니다.

            // Riding 상태라면 Riding 관련 추가 로직도 처리
            if (currentState == PlayerState.Riding)
            {
                HandleRiding();
            }
        }
        Debug.Log("라이딩 상태 : " + currentState);
        Debug.Log("선택된 프리팹 : " + currentObjectPrefab);
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Riding && currentObjectPrefab != null)
        {
            HandleRidingMovement();
        }
        else if (currentState == PlayerState.Idle)
        {
            HandlePlayerMovement();
        }
    }

    // 물리 상태 설정을 위한 헬퍼 메서드
    private void SetPhysicsState(bool enablePhysics, bool isKinematic, bool isTrigger)
    {
        // 메인 컴포넌트 설정
        if (mainRigidbody != null)
        {
            mainRigidbody.useGravity = enablePhysics;
            mainRigidbody.isKinematic = isKinematic;
        }

        if (mainCollider != null)
        {
            mainCollider.isTrigger = isTrigger;
            mainCollider.enabled = true;
        }

        // 모든 래그돌 리지드바디 설정
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null)
            {
                rb.useGravity = enablePhysics;
                rb.isKinematic = isKinematic;
            }
        }

        // 모든 래그돌 콜라이더 설정
        foreach (Collider col in ragdollColliders)
        {
            if (col != null)
            {
                col.isTrigger = isTrigger;
                col.enabled = true;
            }
        }
    }

    private void UpdatePlayerState(PlayerState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                // 일반 상태: 모든 물리 활성화
                SetPhysicsState(true, false, false);
                break;

            case PlayerState.Transitioning:
                // 전환 상태: 모든 물리 비활성화, 키네마틱 활성화
                SetPhysicsState(false, true, true);
                break;

            case PlayerState.Dead:
                // 죽은 상태 : 일반 상태와 같음
                SetPhysicsState(true, false, false);
                break;

            case PlayerState.Riding:
                // 탑승 상태: 모든 콜라이더 비활성화
                DisableAllPhysics();
                break;
        }
    }

    // 모든 물리 컴포넌트 비활성화
    private void DisableAllPhysics()
    {
        // 메인 컴포넌트 비활성화
        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = true;
            mainRigidbody.useGravity = false;
        }
        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        // 모든 래그돌 컴포넌트 비활성화
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

    // 기존의 키 입력 기반 상호작용 함수는 더 이상 사용하지 않습니다.
    // private void HandleInput()
    // {
    //     if (Input.GetKey(interactKeyCode))
    //     {
    //         Debug.Log("상호 작용 키가 눌림");
    //         HandleInteraction();
    //     }
    // }

    // 기존 HandleInteraction()도 사용하지 않으므로 제거 가능함.
    // 필요한 경우 추후 다른 입력 처리로 활용하세요.

    /// <summary>
    /// 플레이어가 다른 오브젝트와 충돌하면, 해당 오브젝트가 InteractableObject라면 탑승 전환을 시작합니다.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // 현재 Idle 상태일 때만 충돌로 탑승을 허용합니다.
        if (currentState != PlayerState.Idle)
            return;

        // 충돌한 오브젝트에서 InteractableObject 컴포넌트를 가져옵니다.
        InteractableObject interactable = collision.gameObject.GetComponent<InteractableObject>();
        if (interactable != null && interactable != currentInteractableObject)
        {
            Debug.Log("충돌하여 상호작용 시작");
            StartTransition(interactable);
        }
    }

    private void StartTransition(InteractableObject target)
    {
        SaveCurrentVelocity(); // 전환 시작 전에 현재 속도 저장

        ExitObject(); // 기존 오브젝트에서 내리기

        UpdatePlayerState(PlayerState.Transitioning);
        targetObject = target;
        startPosition = transform.position;
        transitionTime = 0f;
        lastKnownMountPoint = target.mountPoint.position;

        // 이동 중에는 입력과 이동을 비활성화
        currentInput = null;
        currentMovement = null;
    }

    private void UpdateTransition()
    {
        // targetObject가 없으면 상태를 Idle로 전환하고 함수 종료
        if (targetObject == null)
        {
            currentState = PlayerState.Idle;
            return;
        }

        // 전환 진행 시간 업데이트 및 진행 비율 계산
        transitionTime += Time.deltaTime;
        float normalizedTime = transitionTime / transitionDuration;

        // 목표 위치 (mountPoint) 가져오기
        Vector3 targetPosition = targetObject.mountPoint.position;
        lastKnownMountPoint = targetPosition;

        // 진행 비율에 따라 점프 높이 계산 (사인 함수를 이용해 부드러운 상승/하강 효과)
        float height = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;
        // 시작 위치에서 목표 위치로 선형 보간하고, 위쪽(height) 오프셋을 더하여 현재 위치 계산
        Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, normalizedTime) + Vector3.up * height;
        transform.position = currentPosition;

        // 목표 방향 계산 후, 해당 방향을 향하도록 부드러운 회전 보간 적용
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // 목표 위치와의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // 전환 진행이 완료되었거나 (normalizedTime >= 1.0f)
        // 목표에 충분히 가까워졌으면 (distanceToTarget < mountThreshold) 전환 완료 처리
        if (normalizedTime >= 1.0f || distanceToTarget < mountThreshold)
        {
            CompleteTransition();
        }
    }

    private void CompleteTransition()
    {
        if (targetObject != null)
        {
            transform.position = lastKnownMountPoint;
            transform.rotation = targetObject.mountPoint.rotation;
            EnterObject(targetObject);
        }

        targetObject = null;
    }

    private void EnterObject(InteractableObject interactableObject)
    {
        // 1. 기존 프리팹 제거
        if (currentObjectPrefab != null)
        {
            Destroy(currentObjectPrefab);
            currentObjectPrefab = null;
        }

        // 2. 플레이어 메쉬 렌더러 비활성화
        foreach (SkinnedMeshRenderer skin in skinRenderer)
        {
            skin.enabled = false;
        }

        // 기존에 타고 있던 오브젝트에서 내리는 처리
        if (currentInteractableObject != null)
        {
            currentInteractableObject.gameObject.SetActive(true);
        }

        // 3. 새로운 오브젝트 설정
        currentInteractableObject = interactableObject;

        // 4. 새로운 오브젝트 타기 (프리팹 생성)
        Ride(currentInteractableObject);

        // 프리팹이 생성된 후에 저장된 속도 적용
        ApplySavedVelocity();

        // 5. currentMovement와 currentInput을 프리팹에서 가져오기
        currentMovement = currentObjectPrefab.GetComponentInChildren<IMovement>();
        currentInput = currentObjectPrefab.GetComponentInChildren<IInputHandler>();

        // 6. 상태 업데이트
        UpdatePlayerState(PlayerState.Riding);
        transform.SetParent(currentObjectPrefab.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void ExitObject()
    {
        transform.SetParent(null);
        UpdatePlayerState(PlayerState.Idle);

        foreach (SkinnedMeshRenderer skin in skinRenderer)
        {
            skin.enabled = true;
        }

        // 타고 있는 오브젝트가 있다면, 해당 오브젝트에서 내림
        if (currentInteractableObject != null)
        {
            // 원래 물건 위치와 회전을 프리팹 위치와 회전으로 설정
            currentInteractableObject.transform.position = currentObjectPrefab.transform.position;
            currentInteractableObject.transform.rotation = currentObjectPrefab.transform.rotation;

            // 기존에 타고 있던 오브젝트 다시 활성화
            currentInteractableObject.gameObject.SetActive(true);

            // currentInteractableObject 초기화
            currentInteractableObject = null;

            // 기존 프리팹 제거
            if (currentObjectPrefab != null)
            {
                Destroy(currentObjectPrefab);
                currentObjectPrefab = null;
            }

            // 플레이어의 기본 이동 및 입력 컨트롤러로 복구
            currentMovement = GetComponent<IMovement>();
            currentInput = GetComponent<IInputHandler>();
        }
    }

    private void Ride(InteractableObject target)
    {
        // 타겟 비활성화
        target.gameObject.SetActive(false);

        // 프리팹 생성 위치 및 회전 설정
        Vector3 spawnPosition = target.transform.position;
        Quaternion spawnRotation = target.transform.rotation;

        // 미니게임 결과에 따라 생성되는 프리팹이 달라질 수 있음
        // 아래 코드는 미니게임 승리 프리팹으로 생성하는 예시입니다.
        currentObjectPrefab = Instantiate(target.objectData.winPrefab,
                                         spawnPosition + new Vector3(0, 1f, 0),
                                         spawnRotation);
    }

    // 탈 수 있는 오브젝트 찾는 함수 (현재 사용되지 않음)
    private InteractableObject CheckForInteractableObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
        InteractableObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();

            if (interactable != null && interactable != currentInteractableObject &&
                !(currentObjectPrefab != null && interactable == currentObjectPrefab.GetComponent<InteractableObject>()))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = interactable;
                }
            }
        }

        return closestObject;
    }

    private void HandleRiding()
    {
        // Riding 상태에서의 추가적인 로직 (예: 특정 애니메이션 재생)
    }

    private void HandleRidingMovement()
    {
        if (currentInput != null && currentMovement != null)
        {
            Debug.Log("인풋,무브먼트가 존재함");
            Vector3 moveDirection = currentInput.HandleInput();
            currentMovement.Move(moveDirection);
        }
    }

    private void HandlePlayerMovement()
    {
        if (currentInput != null && currentMovement != null)
        {
            Vector3 moveDirection = currentInput.HandleInput();
            currentMovement.Move(moveDirection);
        }
    }

    // 이벤트가 호출될 때 실행될 메서드
    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Debug.Log("Time scale reset to 1f.");
    }

    // 미니게임의 결과에 따른 프리팹 선택 (현재 Ride()에서 직접 winPrefab 사용)
    private GameObject SelectPrefab(InteractableObject target)
    {
        GameObject Prefab = null;

        if (miniGame.lastCollisionState == sliderM.CollisionState.Win)
        {
            Debug.Log("미니게임 성공");
            Prefab = target.objectData.winPrefab;
        }
        else if (miniGame.lastCollisionState == sliderM.CollisionState.Pass)
        {
            Debug.Log("미니게임 패스");
            Prefab = target.objectData.passPrefab;
            if (Prefab == null)
                Prefab = target.objectData.winPrefab;
        }
        else
        {
            Debug.Log("미니게임 실패");
            // 게임 오버? 떨어지기
            UpdatePlayerState(PlayerState.Dead);
        }

        return Prefab;
    }

    public void durabilityZero()
    {
        UpdatePlayerState(PlayerState.Dead);
    }

    private void SaveCurrentVelocity()
    {
        if (currentObjectPrefab != null)
        {
            Rigidbody rb = currentObjectPrefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                savedVelocity = rb.linearVelocity;
                savedAngularVelocity = rb.angularVelocity;
                hasSavedVelocity = true;
                Debug.Log($"Saved velocity: {savedVelocity}, Angular velocity: {savedAngularVelocity}");
            }
        }
    }

    private void ApplySavedVelocity()
    {
        if (hasSavedVelocity && currentObjectPrefab != null)
        {
            Rigidbody rb = currentObjectPrefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = savedVelocity;
                rb.angularVelocity = savedAngularVelocity;
                Debug.Log($"Applied velocity: {savedVelocity}, Angular velocity: {savedAngularVelocity}");
            }
            hasSavedVelocity = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        if (currentState == PlayerState.Transitioning && targetObject != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetObject.mountPoint.position);
            Gizmos.DrawWireSphere(targetObject.mountPoint.position, mountThreshold);
        }
    }

    private void OnTriggerEnter(Collider other) {
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if(interactable != null)
        {
            Ride(interactable);
        }
    }
}
