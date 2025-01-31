using UnityEngine;
using UnityEngine.UIElements;

public class PlayerKMS: MonoBehaviour
{
    public IInputHandler currentInput;
    public IMovementController currentMovement;
    private InteractableObject currentInteractableObject;

    public float interactionRange = 2f;

    public KeyCode interactKeyCode = KeyCode.G;

    private void Awake()
    {
        //// 기본적인 Movement, Input 초기화
        //currentInput = new ADInputHandler(5f);
        //// 플레이어에 기본적으로 AD 무브먼트가 달려있어야함
        //currentMovement = GetComponent<IMovementController>();
    }

    private void Update()
    {
        if (currentInput != null && currentMovement != null)
        {
            currentMovement.Move(currentInput.HandleInput());
        }

        if (Input.GetKey(interactKeyCode))
        {
            CheckForInteractableObjects();
            EnterObject(currentInteractableObject);
        }
    }

    // 가까이 있는 것중 탈 것 찾기
    private void CheckForInteractableObjects()
    {
        // interactionRange는 물체의 중심에서 찾기 때문에
        // 범위를 그 물체의 가로 세로 범위 + range로 수정하는 함수가 필요해 보임
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);

        InteractableObject closestObject = null;
        float closestDistance = Mathf.Infinity;


        foreach (var hitCollider in hitColliders)
        {
            InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = interactable;
                }
            }
        }

        currentInteractableObject = closestObject;
    }


    // 물체에 붙는 함수
    public void EnterObject(InteractableObject interactableObject)
    {
        if (interactableObject.mountPoint != null)
        {
            transform.position = interactableObject.mountPoint.position;
            transform.rotation = interactableObject.mountPoint.rotation;
        }
        // else문으로 물체를 못찾았을때의 모션 같은걸 보여주면 좋아보임
        ChangeMovement(interactableObject.movementController);
        ChangeInput(interactableObject.inputHandler);

        // 탑승한 뒤 어떻게 할것인지에 대한 것
        //if (interactableObject.mountAnimationName != null && animator != null)
        //{
        //    animator.Play(interactableObject.mountAnimationName);
        //}
    }

    // 물체에 떨어지는 함수
    public void ExitObject()
    {
        if (currentInteractableObject != null)
        {
            currentMovement = GetComponent<IMovementController>();
            
            currentInteractableObject = null;
            //if (animator != null)
            //{
            //    animator.Play("Default");
            //}
        }
    }


    // Movement를 변경하는 함수
    public void ChangeMovement(IMovementController newMovement)
    {
        currentMovement = newMovement;
    }
    
    // inputHandler를 변경하는 함수
    public void ChangeInput(IInputHandler newInputHandler)
    {
        currentInput = newInputHandler;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
