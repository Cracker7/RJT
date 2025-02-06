using UnityEngine;

public class PlayerKMS : MonoBehaviour
{
    // 상태 머신
    private enum PlayerState { Idle, Transitioning, Riding }
    private PlayerState currentState = PlayerState.Idle;

    // 컴포넌트 캐싱
    private MeshRenderer meshRenderer;
    public IInputHandler currentInput;
    public IMovement currentMovement;

    // 상호 작용 관련
    private InteractableObject currentInteractableObject;
    [Header("상호작용 설정")]
    public float interactionRange = 2f;
    public KeyCode interactKeyCode = KeyCode.G;

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

    private void Awake()
    {
        // 컴포넌트 캐싱
        meshRenderer = GetComponent<MeshRenderer>();
        currentMovement = GetComponent<IMovement>();
        currentInput = GetComponent<IInputHandler>();
    }

    private void OnEnable()
    {
        sliderM.OnShutdown += ResetTimeScale;
    }

    private void OnDisable()
    {
        sliderM.OnShutdown -= ResetTimeScale;
    }

    private void Update()
    {
        // Transitioning 상태일 때는 입력을 무시한다.
        if (currentState == PlayerState.Transitioning)
        {
            UpdateTransition();
        }
        else
        {
            // Idle이나 Riding 상태일 때는 항상 인터렉션 입력을 처리하도록 함
            HandleInput();

            // Riding 상태라면 Riding 관련 추가 로직도 처리
            if (currentState == PlayerState.Riding)
            {
                HandleRiding();
            }
        }
        Debug.Log("라이딩 상태 : " + PlayerState.Riding);
        Debug.Log("선택된 프리팹 : " + currentObjectPrefab);
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Riding && currentObjectPrefab != null)
        {
            //transform.position = targetObject.mountPoint.position;
            //transform.rotation = currentInteractableObject.transform.rotation;
            HandleRidingMovement();
        }
        else if (currentState == PlayerState.Idle)
        {
            HandlePlayerMovement();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKey(interactKeyCode))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        InteractableObject nearestObject = CheckForInteractableObjects();

        if ((currentInteractableObject != null && nearestObject != null && nearestObject != currentInteractableObject) ||
            (currentInteractableObject == null && nearestObject != null))
        {
            StartTransition(nearestObject);
        }
    }

    private void StartTransition(InteractableObject target)
    {
        ExitObject(); // 기존 오브젝트에서 내리기

        currentState = PlayerState.Transitioning;
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

        // 최대 높이에 도달했을 때 타임스케일을 느리게 설정 (여기서는 normalizedTime이 0.5 근처일 때)
        // 0.05f의 허용 오차를 두어 0.45 ~ 0.55 구간에서 적용
        if (!hasSlowedTime && Mathf.Abs(normalizedTime - 0.5f) < 0.05f)
        {
            Time.timeScale = timeScale;
            hasSlowedTime = true;
            // 미니게임 시작
            miniGame.OpenCanvas();
            // 이벤트로 미니게임이 끝나면 타임스케일이 되돌아옴.
        }

        // 전환 진행이 완료되었거나 (normalizedTime >= 1.0f)
        // 목표에 충분히 가까워졌으면 (distanceToTarget < mountThreshold) 전환 완료 처리
        if (normalizedTime >= 1.0f || distanceToTarget < mountThreshold)
        {
            CompleteTransition();
            hasSlowedTime = false;
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

        //currentState = PlayerState.Idle;
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
        meshRenderer.enabled = false;

        // 기존에 타고 있던 오브젝트에서 내리는 처리
        if (currentInteractableObject != null)
        {
            currentInteractableObject.gameObject.SetActive(true);
        }

        // 3. 새로운 오브젝트 설정
        currentInteractableObject = interactableObject;

        // 4. 새로운 오브젝트 타기 (프리팹 생성)
        Ride(currentInteractableObject);

        // 5. currentMovement와 currentInput을 프리팹에서 가져오기
        currentMovement = currentObjectPrefab.GetComponentInChildren<IMovement>();
        currentInput = currentObjectPrefab.GetComponentInChildren<IInputHandler>();

        // 6. 상태 업데이트
        currentState = PlayerState.Riding;

        transform.SetParent(currentObjectPrefab.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void ExitObject()
    {
        transform.SetParent(null);

        meshRenderer.enabled = true;
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

        //// 새로운 프리팹 생성
        //Vector3 spawnPosition = target.mountPoint.position;
        //Quaternion spawnRotation = target.mountPoint.rotation;
        // 새로운 프리팹 생성
        Vector3 spawnPosition = target.transform.position;
        Quaternion spawnRotation = target.transform.rotation;

        // 미니게임 결과에 따라 생성되는 프리팹이 달라야함
        currentObjectPrefab = Instantiate(SelectPrefab(target),
                                         spawnPosition + new Vector3(0,0.1f,0),
                                         spawnRotation);
    }

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

    // 미니게임의 결과에 따른 프리팹 선택
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
        }

        return Prefab;
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
}