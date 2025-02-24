using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKMS : MonoBehaviour
{
    public static int DeadCnt = 0;
    public CinemachineCamera JumpCam;
    public CinemachineCamera followCam;
    public Transform followPlayer;
    // 상태 머신
    public enum PlayerState { Idle, Transitioning, Riding, Dead }
    public PlayerState currentState = PlayerState.Idle;

    // 컴포넌트 캐싱
    private List<SkinnedMeshRenderer> skinRenderer;
    private InteractableObject currentInteractable;
    private Rigidbody cachedVehicleRigidbody;
    public IInputHandler currentInput;
    public IMovement currentMovement;
    public IBoxCastFinder boxCastFinder;
    public RGTCarDownV2 carDown;

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
    private bool Isdurabillity = false;

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

    [Header("내구도 0 발사체 설정")]
    public float projectileDuration = 1f;     // 전체 발사체 이동 시간 (초)
    public float projectileDistance = 10f;      // 앞으로 날아갈 거리
    public float projectileHeight = 3f;         // 최고 높이 (조정 가능)
    private bool isProjectileLaunched = false;  // 발사체 모션 진행 여부
    private float projectileElapsedTime = 0f;   // 경과 시간
    private Vector3 projectileStartPosition;    // 발사 시작 위치
    private Vector3 projectileForward;          // 발사 시의 진행 방향

    public GameObject BoomGo;
    public RectTransform AD;
    public RectTransform Arrow;
    public RectTransform ADObjection;
    public RectTransform ArrowObjection;

    [Space(10)]
    public GameObject currentObjectPrefab;
    // currentObjectPrefab 대신 ActiveRigidbody를 사용하여
    // 탑승한 오브젝트의 리지드바디가 있다면 그것을, 없으면 플레이어 자신의 리지드바디를 반환
    public Rigidbody ActiveRigidbody
    {
        get
        {
            return (currentObjectPrefab != null && cachedVehicleRigidbody != null) ?
                    cachedVehicleRigidbody : mainRigidbody;
        }
    }
    //public sliderM miniGame;

    // 속도 전달 관련
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;
    private bool hasSavedVelocity = false;

    private bool hasHandledRiding = false;

    private bool isDead = false;
    private bool hasTransition = false;

    private Type[] handlerTypes;

    private void Awake()
    {
        // 기본 컴포넌트 캐싱
        skinRenderer = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
        currentMovement = GetComponent<IMovement>();
        currentInput = GetComponent<IInputHandler>();
        boxCastFinder = new BoxCastFinder();

        // 래그돌 컴포넌트 캐싱
        CacheRagdollComponents();

        // 초기 물리 상태 설정
        SetPhysicsState(true, false, false);

        // IInputHandler를 구현하는 모든 타입을 찾습니다.
        handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IInputHandler).IsAssignableFrom(type)
                           && type.IsClass
                           && !type.IsAbstract
                           && typeof(MonoBehaviour).IsAssignableFrom(type))
            .ToArray();
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
        sliderM.OnShutdown += ResetTimeScale;
    }

    private void OnDisable()
    {
        sliderM.OnShutdown -= ResetTimeScale;
    }

    private void Start()
    {
        StartTransition(CheckForInteractableObjects());
    }

    private void FixedUpdate()
    {
        // Transitioning 상태일 때는 입력을 무시한다.
        if (currentState == PlayerState.Transitioning)
        {
            UpdateTransition();
        }
        else if (currentState == PlayerState.Dead)
        {
            // 플레이어가 죽었을 때 실행하는 함수
            if (currentObjectPrefab == null)
            {
                if (!isDead)
                {
                    HandleInput();
                }
                else
                {
                    // 죽으면 UI가 뜸 로비 돌아가기 재시작하기
                    // 이때 다른 UI들은 꺼져야함
                    Invoke("Restart", 0.5f);
                }
            }
        }
        else
        {
            HandleInput();

            // Riding 상태라면 Riding 관련 추가 로직도 처리
            if (currentState == PlayerState.Riding)
            {
                if (!hasHandledRiding && currentInteractable.currentDurability <= currentInteractable.maxDurability / 2)
                {
                    HandleRiding();
                    hasHandledRiding = true;
                }
            }
        }
        Debug.Log("라이딩 상태 : " + currentState);
        Debug.Log("선택된 프리팹 : " + currentObjectPrefab);

        if (currentState == PlayerState.Riding && currentObjectPrefab != null)
        {
            HandleRidingMovement();
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
            //mainCollider.isTrigger = isTrigger;
            Invoke("TriggerOn", 0.5f);
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
                // 카메라 떨림 방지 메인 리지드바디 속도 0;
                if (mainRigidbody != null)
                {
                    mainRigidbody.linearVelocity = Vector3.zero;
                    mainRigidbody.angularVelocity = Vector3.zero;
                }

                // 카메라 떨리는걸 방지하기 위해서 속도를 0으로 만들어줌
                foreach (Rigidbody rb in ragdollRigidbodies)
                {
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }
                }
                SetPhysicsState(false, true, false);


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
        // 캐릭터 상태가 변한 뒤 UI 업데이트
        UpdateUI();
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
    private void HandleInput()
    {
        if (Input.GetKey(interactKeyCode))
        {
            Debug.Log("상호 작용 키가 눌림");
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        InteractableObject nearestObject = CheckForInteractableObjects();

        if ((currentInteractableObject != null && nearestObject != null && nearestObject != currentInteractableObject) ||
            (currentInteractableObject == null && nearestObject != null))
        {
            Debug.Log("주변 오브젝트를 찾음");
            StartTransition(nearestObject);
            // 미니 게임 열기
            //miniGame.OpenCanvas();
        }
    }

    public void StartTransition(InteractableObject target)
    {
        hasHandledRiding = false;

        SaveCurrentVelocity(); // 전환 시작 전에 현재 속도 저장

        ExitObject(); // 기존 오브젝트에서 내리기

        UpdatePlayerState(PlayerState.Transitioning);

        hasTransition = true;
        targetObject = target;
        startPosition = transform.position;
        transitionTime = 0f;

        // 목표 위치 (mountPoint) 가져오기
        //Vector3 targetPosition = targetObject.mountPoint.position;
        lastKnownMountPoint = targetObject.mountPoint.position;

        // 이동 중에는 입력과 이동을 비활성화
        currentInput = null;
        currentMovement = null;
        followCam.Follow = null;
        StartCoroutine(SwitchCameraWithDelay());
        StartCoroutine(CameraZoomEffect());
        StartCoroutine(SwitchCameraWithDelay());
        //JumpCam.Priority = 15;
        Time.timeScale = 0.5f;
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
        transitionTime += Time.fixedDeltaTime;
        float normalizedTime = transitionTime / transitionDuration;

        // SmoothStep을 사용하여 더 부드러운 보간
        float smoothTime = Mathf.SmoothStep(0f, 1f, normalizedTime);

        // 진행 비율에 따라 점프 높이 계산 (사인 함수를 이용해 부드러운 상승/하강 효과)
        float height = Mathf.Sin(smoothTime * Mathf.PI) * jumpHeight;
        // 시작 위치에서 목표 위치로 선형 보간하고, 위쪽(height) 오프셋을 더하여 현재 위치 계산
        Vector3 currentPosition = Vector3.Lerp(startPosition, lastKnownMountPoint, normalizedTime) + Vector3.up * height;
        transform.position = currentPosition;

        // 목표 방향 계산 후, 해당 방향을 향하도록 부드러운 회전 보간 적용
        Vector3 direction = (lastKnownMountPoint - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        // 목표 위치와의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, lastKnownMountPoint);

        // 전환 진행이 완료되었거나 (normalizedTime >= 1.0f)
        // 목표에 충분히 가까워졌으면 (distanceToTarget < mountThreshold) 전환 완료 처리
        if (normalizedTime >= 1.0f || distanceToTarget < mountThreshold)
        {
            //// 미니 게임을 실패했을 경우의 코드
            //if (miniGame.lastCollisionState == sliderM.CollisionState.Fail)
            //{
            //    ExitObject();
            //    UpdatePlayerState(PlayerState.Dead);
            //    ExplosionRb();
            //}
            //else
            CompleteTransition();
            // 미니 게임 닫는 코드
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
        JumpCam.Priority = 5;
        Time.timeScale = 1f;
    }

    // 탈것에 탑승하는 처리 함수
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

    public void ExitObject()
    {
        if (currentObjectPrefab != null)
        {
            Instantiate(BoomGo, transform.position, Quaternion.identity);
        }
        transform.SetParent(null, true);
        UpdatePlayerState(PlayerState.Idle);

        foreach (SkinnedMeshRenderer skin in skinRenderer)
        {
            skin.enabled = true;
        }

        // 타고 있는 오브젝트가 있다면, 해당 오브젝트에서 내림
        if (currentInteractableObject != null)
        {
            if (currentInteractable == null) return;

            ExitEvent();

            currentInteractable = null;

            // 원래 물건 위치와 회전을 프리팹 위치와 회전으로 설정
            currentInteractableObject.transform.position = currentObjectPrefab.transform.position;
            currentInteractableObject.transform.rotation = currentObjectPrefab.transform.rotation;

            if (Isdurabillity)
            {
                currentInteractableObject.gameObject.SetActive(false);
                Isdurabillity = false;
            }
            else if (hasHandledRiding)
            {
                // 기존에 타고 있던 오브젝트 다시 활성화
                currentInteractableObject.gameObject.SetActive(true);
            }

            // currentInteractableObject 초기화
            currentInteractableObject = null;

            // 기존 프리팹 제거
            if (currentObjectPrefab != null)
            {
                Destroy(currentObjectPrefab);
                currentObjectPrefab = null;
            }

            // 캐싱된 탑승 오브젝트의 리지드바디 초기화
            cachedVehicleRigidbody = null;

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
        Vector3 spawnPosition = target.mountPoint.position;
        Quaternion spawnRotation = target.mountPoint.rotation;

        // 미니게임 결과에 따라 생성되는 프리팹이 달라질 수 있음
        currentObjectPrefab = Instantiate(target.objectData.Prefab,
                                         spawnPosition,
                                         spawnRotation);

        // 캐싱: 생성된 프리팹의 Rigidbody를 한 번 가져와서 저장
        cachedVehicleRigidbody = currentObjectPrefab.GetComponent<Rigidbody>();

        // 현재 타고 있는 오브젝트의 인터렉테이블 오브젝트
        currentInteractable = currentObjectPrefab.GetComponent<InteractableObject>();
        EnterEvent();

        // 현재 타고 있는 오브젝트의 파괴 이벤트
        currentInteractable.OnDestroyCalled += durabilityZero;
    }

    // 탈 수 있는 오브젝트 찾는 함수
    private InteractableObject CheckForInteractableObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
        InteractableObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            // 플레이어와 오브젝트 사이의 방향 계산
            Vector3 directionToObject = (hitCollider.transform.position - transform.position).normalized;
            // 플레이어의 forward 벡터와의 내적값이 음수이면 플레이어 뒤쪽에 있는 것
            // 그 오브젝트는 무시
            if (Vector3.Dot(transform.forward, directionToObject) < 0)
                continue;

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

    // 타고 있는 상태에서 내구도가 깍였을 경우
    // 애매한 탈것으로 변경하는 함수
    private void HandleRiding()
    {
        //// Riding 상태일때 내구도가 일정 이하로 내려가면 애매한것으로 변경되도록 함
        //StartTransition(currentInteractableObject);
        ////targetObject.objectData.passPrefab;
        // 현재 타고 있는 오브젝트가 존재할 때만 실행
        if (currentInteractable != null)
        {
            // ExitObject() 호출 시 currentInteractableObject가 null로 초기화되므로 미리 저장
            InteractableObject target = currentInteractable;
            float durability = currentInteractable.currentDurability;
            float maxDurability = currentInteractable.maxDurability;

            // 만약 속도 전달이 필요하다면 속도 저장
            SaveCurrentVelocity();

            // 기존 오브젝트에서 내리기 (이 과정에서 currentInteractableObject가 null로 초기화됨)
            ExitObject();

            // 저장해둔 대상(target)을 이용해 바로 오브젝트를 타도록 합니다.
            EnterObject(target);

            // 이전 내구도를 현재 탑승한 애매한 탈것에 계승
            currentInteractable.maxDurability = maxDurability;
            currentInteractable.currentDurability = durability;
            currentInteractable.hpBar.UpdateHpBar(maxDurability, durability);

            // 탈것의 입력 스크립트가 없을 경우 랜덤적으로 추가하도록 만듬
            if (currentInput == null)
            {
                Debug.Log("랜덤 실행됨");
                currentInput = AddRandomInputHandler(currentObjectPrefab);
                UpdateUI();
            }
        }
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

    // 이벤트가 호출될 때 실행될 메서드
    private void ResetTimeScale()
    {

    }

    // 미니게임의 결과에 따른 프리팹 선택 (현재 Ride()에서 직접 winPrefab 사용)
    //private GameObject SelectPrefab(InteractableObject target)
    //{
    //    GameObject Prefab = null;

    //    if (miniGame.lastCollisionState == sliderM.CollisionState.Win)
    //    {
    //        Debug.Log("미니게임 성공");
    //        Prefab = target.objectData.Prefab;
    //    }
    //    else if (miniGame.lastCollisionState == sliderM.CollisionState.Pass)
    //    {
    //        Debug.Log("미니게임 패스");
    //        Prefab = target.objectData.passPrefab;
    //        if (Prefab == null)
    //            Prefab = target.objectData.winPrefab;
    //    }
    //    //else if (miniGame.lastCollisionState == sliderM.CollisionState.Fail)
    //    //{
    //    //    Debug.Log("미니게임 실패");
    //    //    // 게임 오버? 떨어지기
    //    //    UpdatePlayerState(PlayerState.Dead);
    //    //}

    //    return Prefab;
    //}

    public void durabilityZero()
    {
        if (currentState == PlayerState.Transitioning) return;
        Isdurabillity = true;
        ExitObject();
        //LaunchProjectileMotion(); // 발사체 모션 시작
        ExplosionRb();
        currentState = PlayerState.Dead;
        UpdateUI();
        Invoke("TriggerOn", 0.5f);
    }

    public void SetDeadState()
    {
        ExitObject();
        //LaunchProjectileMotion(); // 발사체 모션 시작
        ExplosionRb();
        //UpdatePlayerState(PlayerState.Dead);
        currentState = PlayerState.Dead;
        UpdateUI();
        Invoke("TriggerOn", 0.5f);
    }

    private void LaunchProjectileMotion()
    {
        // 상태는 이미 ExitObject에서 Idle로 전환되었지만 혹시 몰라 다시 설정
        UpdatePlayerState(PlayerState.Idle);
        // 현재 위치와 진행 방향 저장
        projectileStartPosition = transform.position;
        projectileForward = transform.forward; // 발사 시점의 forward 방향 저장
        projectileElapsedTime = 0f;
        isProjectileLaunched = true;
    }

    private void UpdateProjectileMotion()
    {
        projectileElapsedTime += Time.deltaTime;
        float t = projectileElapsedTime / projectileDuration;
        t = Mathf.Clamp01(t);

        // 수평 이동: 발사 시 저장한 forward 방향을 따라 선형 진행
        Vector3 horizontalOffset = projectileForward * projectileDistance * t;

        // 수직 이동: 포물선 (예시: 4 * H * t * (1-t)) — t=0,1에서 0, t=0.5에서 최고 높이
        float verticalOffset = 4f * projectileHeight * t * (1f - t);

        // 새로운 위치 계산 (시작 위치 기준)
        transform.position = projectileStartPosition + horizontalOffset + Vector3.up * verticalOffset;

        // 이동 완료 후 발사체 모션 종료
        if (t >= 1f)
        {
            isProjectileLaunched = false;
        }
    }

    private void ExplosionRb()
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb != null)
            {
                rb.AddExplosionForce(100f, transform.position, 20f, 20f, ForceMode.Impulse);
            }
        }
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
                if (hasTransition)
                {
                    Debug.Log("갈아탔을때 가속");
                    AccelVelocity(rb);
                    rb.angularVelocity = savedAngularVelocity;

                    // 트랜지션을 거친 상태를 다시 초기화
                    hasTransition = false;
                }
                else
                {
                    rb.linearVelocity = savedVelocity;
                    rb.angularVelocity = savedAngularVelocity;
                    Debug.Log($"Applied velocity: {savedVelocity}, Angular velocity: {savedAngularVelocity}");
                }
            }
            hasSavedVelocity = false;
        }
    }

    private void AccelVelocity(Rigidbody rb)
    {
        // 속도가 무제한으로 늘어날 수 있기 때문에 가속할 수 있는 최대 속도는 지정
        rb.linearVelocity = Vector3.ClampMagnitude(savedVelocity * 4.5f, 300f);
        if (rb.linearVelocity.magnitude <= 100f)
            rb.linearVelocity = savedVelocity.normalized * 70f;
        // Z 방향으로만 가속할 경우 아래로 내려갈때 또는 위로 올라갔을때 갈아탈경우
        // 값이 너무 차이나서 평면으로 이동하게 되어버림
        //float zAccelVelocity = savedVelocity.z * 5;
        //rb.linearVelocity = new Vector3(savedVelocity.x, savedVelocity.y, zAccelVelocity);

        // 힘을 가하여 가속
        //float accelerationForce = 50f; // 힘의 크기 조정
        //rb.AddForce(savedVelocity.normalized * accelerationForce, ForceMode.Acceleration);

        // 현재 속도 방향으로 가속
        //Vector3 currentVelocityDirection = savedVelocity.normalized;
        //float accelerationMagnitude = 250f; // 가속 크기 조정

        //rb.AddForce(currentVelocityDirection * accelerationMagnitude, ForceMode.VelocityChange); // VelocityChange 모드 사용
    }
    private void EnterEvent()
    {
        currentInteractable.onRideUpdate += currentInteractable.StartHpDecrease;
        currentInteractable.onRideUpdate.Invoke();
        currentInteractable.onRideCol += currentInteractable.HandleCollisionDamage;
        currentInteractable.OnFrontalCollision += SetDeadState;
        carDown = new RGTCarDownV2();
        carDown.Die += SetDeadState;
    }

    private void ExitEvent()
    {
        // 현재 탄 물체의 이벤트 삭제
        currentInteractable.onRideUpdate -= currentInteractableObject.StartHpDecrease;
        currentInteractable.OnDestroyCalled -= durabilityZero;
        currentInteractable.onRideCol -= currentInteractable.HandleCollisionDamage;
        currentInteractable.OnFrontalCollision -= SetDeadState;
        carDown.Die -= SetDeadState;
        carDown = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("플레이어와 부딪힌 것 이름" + other.name);
        //if (other.CompareTag("Untagged")) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Default")) return;
        if (currentState == PlayerState.Dead)
        {
            Debug.Log("플레이어가 죽음");
            isDead = true;
            ++DeadCnt;
        }
    }

    private void TriggerOn()
    {
        mainCollider.enabled = true;
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

    private IEnumerator SwitchCameraWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        followCam.Lens.FieldOfView = 60;
        //followCam.Follow = followPlayer;
        JumpCam.Priority = 15;
        Time.timeScale = 1;
    }

    private void HideAllUI()
    {
        AD.gameObject.SetActive(false);
        Arrow.gameObject.SetActive(false);
        ADObjection.gameObject.SetActive(false);
        ArrowObjection.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        // Riding 상태가 아니거나 현재 IInputHandler가 없다면 모든 UI를 끕니다.
        if (currentState != PlayerState.Riding || currentInput == null)
        {
            HideAllUI();
            return;
        }

        InputType inputType = currentInput.Type; // InputType은 enum

        // 우선 모든 UI를 비활성화
        HideAllUI();

        // inputType에 따른 UI 활성화 (예시는 상황에 맞게 수정)
        switch (inputType)
        {
            case InputType.AD:
                AD.gameObject.SetActive(true);
                break;
            case InputType.Arrow:
                Arrow.gameObject.SetActive(true);
                break;
            case InputType.ArrowObjection:
                ArrowObjection.gameObject.SetActive(true);
                break;
            case InputType.ADObjection:
                ADObjection.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator SmoothZoomIn(float zoomFOV, float duration)
    {
        float startFOV = followCam.Lens.FieldOfView;
        float time = 0f;

        while (time < duration)
        {
            followCam.Lens.FieldOfView = Mathf.Lerp(startFOV, zoomFOV, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        followCam.Lens.FieldOfView = zoomFOV;
    }

    IEnumerator CameraZoomEffect()
    {
        yield return StartCoroutine(SmoothZoomIn(20f, 0.2f)); // 0.5초 동안 줌인
        //yield return new WaitForSeconds(0.2f); // 폭발 장면 유지
        followCam.Follow = followPlayer;
        yield return StartCoroutine(SmoothZoomIn(60f, 0.2f)); // 다시 원래대로
    }

    private IInputHandler AddRandomInputHandler(GameObject target)
    {

        if (handlerTypes.Length == 0)
        {
            Debug.LogWarning("구현된 IInputHandler 스크립트가 없습니다.");
            return null;
        }

        // 랜덤하게 하나의 타입을 선택합니다.
        var randomType = handlerTypes[UnityEngine.Random.Range(0, handlerTypes.Length)];

        // 선택된 타입을 컴포넌트로 추가합니다.
        IInputHandler inputHandler = (IInputHandler)target.AddComponent(randomType);
        Debug.Log($"추가된 입력 핸들러: {randomType.Name}");

        return inputHandler;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

