using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RGTMouse : MonoBehaviour
{

    //public float forwardForce = 10f;  // 이동 힘
    //public float torqueForce = 5f;    // 회전 힘
    //public float rotationDamping = 0.95f; // 회전 감속 계수

    //private Rigidbody rollingRigidbody;
    //private bool isDragging = false;
    //private Vector3 lastMousePosition;

    //void Start()
    //{
    //    rollingRigidbody = GetComponent<Rigidbody>();
    //}

    //void Update()
    //{
    //    // 마우스 버튼을 누르면 드래그 시작
    //    if (Input.GetMouseButton(0))
    //    {
    //        isDragging = true;
    //        lastMousePosition = Input.mousePosition;
    //    }

    //    // 마우스를 떼면 드래그 종료
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        isDragging = false;
    //    }

    //    // 마우스 드래그 중일 때 이동 및 회전
    //    if (isDragging)
    //    {
    //        Vector3 mouseDelta = Input.mousePosition - lastMousePosition; // 마우스 이동 거리 계산

    //        if(mouseDelta.x > 0)
    //        {

    //        }

    //        rollingRigidbody.AddForce(Vector3.right * mouseDelta.x * forwardForce * Time.deltaTime, ForceMode.Force);
    //        rollingRigidbody.AddTorque(Vector3.up * mouseDelta.x * torqueForce * Time.deltaTime, ForceMode.Force);

    //        lastMousePosition = Input.mousePosition; // 현재 마우스 위치 저장
    //    }

    //    // 마우스를 놓으면 점진적으로 회전 감속
    //    if (!isDragging)
    //    {
    //        rollingRigidbody.angularVelocity *= rotationDamping;
    //    }
    //}



    //마우스를 클릭했다. 그리고 레이를 통해 밀짚모자 인식한다. 밀짚모자의 x 값이 마우스의 x값이 따라 가게 한다.

    //private bool isHat;
    //public Camera mainCamera; // 카메라
    //public float moveSpeed = 10f; // 이동 속도

    //private Rigidbody rb;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    //}
    //private void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            GameObject clickedObject = hit.collider.gameObject;
    //            if (clickedObject.tag == "hat")
    //            {
    //                isHat = true;

    //            }
    //            if (isHat)
    //            {
    //                //Vector3 objectPosition = clickedObject.transform.position; // 현재 위치
    //                //objectPosition.x = hit.point.x; // 마우스 위치의 x 값 적용
    //                //clickedObject.transform.position = objectPosition; // 위치 업데이트

    //                Vector3 targetPosition = hit.point; // 충돌한 지점
    //                Vector3 direction = targetPosition - transform.position; // 현재 위치와 목표 위치 간의 벡터

    //                // 목표 위치로 이동 (Rigidbody를 사용하여 물리적 이동)
    //                rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);
    //            }
    //        }
    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        isHat = false;
    //    }

    //}



    //void Update()
    //{
    //    // 마우스 위치를 월드 좌표로 변환 (Z값은 카메라와의 거리)
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        Vector3 targetPosition = hit.point; // 충돌한 지점
    //        Vector3 direction = targetPosition - transform.position; // 현재 위치와 목표 위치 간의 벡터

    //        // 목표 위치로 이동 (Rigidbody를 사용하여 물리적 이동)
    //        rb.MovePosition(transform.position + direction.normalized * moveSpeed * Time.deltaTime);
    //    }
    //}




    //private Rigidbody rb;
    //private bool isDragging = false;
    //private Vector3 offset;

    //public Camera mainCamera; // 카메라

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
    //}

    //void OnMouseDown()
    //{
    //    // 마우스를 클릭한 물체와의 차이를 계산해서 드래그할 때 마우스와 물체의 상대 위치를 유지
    //    offset = transform.position - GetMouseWorldPos();
    //    isDragging = true;
    //}

    //void OnMouseUp()
    //{
    //    isDragging = false; // 마우스를 떼면 드래그 종료
    //}

    //void Update()
    //{
    //    if (isDragging)
    //    {
    //        // 마우스 위치를 따라 움직이도록 Rigidbody를 사용해 물리적으로 이동
    //        Vector3 targetPosition = GetMouseWorldPos() + offset;
    //        Vector3 direction = targetPosition - transform.position;

    //        // X, Z축으로만 이동 (Y축은 물체가 굴러가도록 유지)
    //        targetPosition.y = transform.position.y;

    //        rb.MovePosition(targetPosition); // 물리적 이동
    //    }
    //}

    //Vector3 GetMouseWorldPos()
    //{
    //    // 마우스 위치를 월드 좌표로 변환 (Z값은 카메라에서 물체까지의 거리)
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        return hit.point;
    //    }

    //    return Vector3.zero;
    //}

    public Camera mainCamera;  // 메인 카메라
    private Rigidbody rb;  // 현재 선택한 물체의 Rigidbody
    private SpringJoint springJoint;  // 마우스로 잡을 때 사용할 SpringJoint
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 눌렀을 때
        {
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스를 놓았을 때
        {
            ReleaseObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Rigidbody hitRb = hit.rigidbody;

            if (hitRb != null && !hitRb.isKinematic) // Rigidbody가 있고, 물리적으로 움직일 수 있는 경우
            {
                rb = hitRb;

                // SpringJoint가 없으면 추가
                if (springJoint == null)
                {
                    GameObject jointHolder = new GameObject("SpringJointHolder");
                    //jointHolder.transform.position = hit.point;
                    springJoint = jointHolder.AddComponent<SpringJoint>();
                    springJoint.connectedBody = rb;
                    springJoint.autoConfigureConnectedAnchor = false;
                    springJoint.spring = 200f; // 스프링 강도 (값을 조정 가능)
                    springJoint.damper = 10f; // 감쇠 (너무 튀지 않도록)
                    springJoint.maxDistance = 0.01f; // 최대 거리 제한
                }
                //rb.position = hit.point;
                //SPJ.x = hit.point.x;
                //springJoint.transform.position = hit.point;

                Vector3 SPJ = springJoint.transform.position;
                SPJ.x = hit.point.x;
                springJoint.transform.position = SPJ;
                springJoint.connectedAnchor = rb.transform.InverseTransformPoint(hit.point);

                rb.freezeRotation = true;
                isDragging = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDragging && springJoint != null)
        {
            // 마우스 위치를 따라가도록 SpringJoint 위치 업데이트
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                springJoint.transform.position = hit.point;
            }
        }
    }

    void ReleaseObject()
    {
        isDragging = false;
        if (springJoint != null)
        {
            Destroy(springJoint.gameObject); // SpringJoint 제거
            springJoint = null;
        }
        if (rb != null)
        {
            rb.freezeRotation = false; // 회전 제한 해제
        }
        rb = null;
    }


}
