using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
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
    public float interactionRange = 2f;
    public KeyCode interactKeyCode = KeyCode.G;

    // 포물선 이동 관련
    private Vector3 startPosition;
    private float transitionTime = 0f;
    public float transitionDuration = 1f;
    public float jumpHeight = 3f;
    public float mountThreshold = 0.5f;
    private InteractableObject targetObject = null;
    private Vector3 lastKnownMountPoint;
    public GameObject currentObjectPrefab;

    private void Awake()
    {
        // 컴포넌트 캐싱
        meshRenderer = GetComponent<MeshRenderer>();
        currentMovement = GetComponent<IMovement>();
        currentInput = GetComponent<IInputHandler>();
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
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Riding && currentObjectPrefab != null)
        {
            transform.position = currentObjectPrefab.transform.position;
            transform.rotation = currentObjectPrefab.transform.rotation;
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
        if (targetObject == null)
        {
            currentState = PlayerState.Idle;
            return;
        }

        transitionTime += Time.deltaTime;
        float normalizedTime = transitionTime / transitionDuration;

        Vector3 targetPosition = targetObject.mountPoint.position;
        lastKnownMountPoint = targetPosition;

        float height = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;
        Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, normalizedTime) + Vector3.up * height;

        transform.position = currentPosition;

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

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
        currentMovement = currentObjectPrefab.GetComponent<IMovement>();
        currentInput = currentObjectPrefab.GetComponent<IInputHandler>();

        // 6. 상태 업데이트
        currentState = PlayerState.Riding;

    }

    private void ExitObject()
    {
        // 타고 있는 오브젝트가 있다면, 해당 오브젝트에서 내림
        if (currentInteractableObject != null)
        {
            // 플레이어 메쉬 렌더러 활성화
            meshRenderer.enabled = true;

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
        // 플레이어 메쉬 렌더러 비활성화
        meshRenderer.enabled = false;

        // 타겟 비활성화
        target.gameObject.SetActive(false);

        //// 새로운 프리팹 생성
        //Vector3 spawnPosition = target.mountPoint.position;
        //Quaternion spawnRotation = target.mountPoint.rotation;
        // 새로운 프리팹 생성
        Vector3 spawnPosition = target.transform.position;
        Quaternion spawnRotation = target.transform.rotation;

        currentObjectPrefab = Instantiate(target.objectData.ridePrefab,
                                         spawnPosition+new Vector3(0,0.1f,0),
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