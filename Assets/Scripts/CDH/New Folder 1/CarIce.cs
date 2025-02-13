using UnityEngine;
using UnityEngine.Splines;

public class CarIce : MonoBehaviour
{
    public Rigidbody carrb;
    //public GameObject playerObject;
    private Rigidbody Ragrb;
    public float slideForce = 5f; // 기본 미끄러짐 힘
    public float minSlideAngle = 10f; // 최소 미끄러질 경사 각도
    public float maxSlideMultiplier = 2f; // 최대 미끄러지는 힘 증가량


    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            Ragrb = playerObject.GetComponent<Rigidbody>();
        }
    }


    //private void Update()
    //{
    //    Debug.DrawLine(transform.position, Vector3.down * 2f, Color.red);
    //    Debug.Log("이 스크립트 쓰이고 있음");
    //}

    //    void FixedUpdate()
    //    {
    //        Debug.Log("빙판길 올라옴");

    //        Debug.DrawLine(transform.position, Vector3.down * 2f, Color.red);

    //        Debug.Log("차속력" + carrb.linearVelocity.magnitude);
    //        Debug.Log("사람속력" + Ragrb.linearVelocity.magnitude);

    //        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
    //        {

    //            Debug.Log("빙판길 올라옴2");
    //            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
    //            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;

    //            Debug.DrawRay(hit.point, hit.normal * 1f, Color.green);



    //            // 경사가 일정 이상이고, 속도가 낮으면 미끄러짐
    //            if (slopeAngle > minSlideAngle && carrb.linearVelocity.magnitude < 2f)
    //            {
    //                float slideMultiplier = Mathf.Clamp(slopeAngle / 45f, 1f, maxSlideMultiplier); // 경사각에 따라 힘 증가
    //                carrb.AddForce(slopeDirection * (slideForce * slideMultiplier), ForceMode.Force);
    //            }
    //        }
    //    }

    //}


    ////using UnityEngine;

    ////public class CarIce : MonoBehaviour
    ////{
    ////    public float slideForce = 10f; // 미끄러짐의 기본 힘
    ////    public float minSlideAngle = 15f; // 미끄러짐을 시작할 최소 경사각
    ////    public float maxSlideMultiplier = 2f; // 최대 미끄러짐 강도
    ////    public float speedLimit = 3f; // 속도 제한

    ////    public Rigidbody rb; // 자전거의 Rigidbody 컴포넌트

    ////    void Start()
    ////    {
    ////        // Rigidbody 컴포넌트 가져오기
    ////        //rb = GetComponent<Rigidbody>();

    ////        // Rigidbody 보간 설정 (부드러운 물리 계산)
    ////        rb.interpolation = RigidbodyInterpolation.Interpolate;
    ////    }

    ////    void FixedUpdate()
    ////    {
    ////        // 경사면을 감지하기 위한 Raycast
    ////        Debug.DrawLine(transform.position, Vector3.down * 2f, Color.red);

    ////        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
    ////        {
    ////            // 경사각 계산
    ////            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
    ////            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;

    ////            // 경사면을 따라 미끄러지는 방향 시각화
    ////            Debug.DrawRay(hit.point, hit.normal * 1f, Color.green);

    ////            // 경사각이 일정 이상이고, 속도가 낮으면 미끄러짐
    ////            if (slopeAngle > minSlideAngle && rb.linearVelocity.magnitude < speedLimit)
    ////            {
    ////                // 미끄러짐 강도 계산 (경사각에 비례)
    ////                float slideMultiplier = Mathf.Clamp(slopeAngle / 45f, 1f, maxSlideMultiplier);

    ////                // 미끄러짐 힘 계산
    ////                float slideForceToApply = slideForce * slideMultiplier;

    ////                // 미끄러짐 힘을 부드럽게 적용
    ////                rb.AddForce(slopeDirection * slideForceToApply * Time.deltaTime, ForceMode.Force);
    ////            }
    ////        }

    ////        // 자전거의 속도 제한 (너무 빠르게 미끄러지지 않도록)
    ////        if (rb.linearVelocity.magnitude > speedLimit)
    ////        {
    ////            rb.linearVelocity = rb.linearVelocity.normalized * speedLimit;
    ////        }
    ////    }
    ////}


    ////using UnityEngine;

    ////public class CarIce : MonoBehaviour
    ////{
    ////    public float slideForce = 10f; // 미끄러짐의 기본 힘
    ////    public float minSlideAngle = 15f; // 미끄러짐을 시작할 최소 경사각
    ////    public float maxSlideMultiplier = 2f; // 최대 미끄러짐 강도
    ////    public float speedLimit = 3f; // 속도 제한

    ////    public Rigidbody rb; // 자전거의 Rigidbody 컴포넌트

    ////    void Start()
    ////    {
    ////        // Rigidbody 컴포넌트 가져오기
    ////        //rb = GetComponent<Rigidbody>();

    ////        // Rigidbody 보간 설정 (부드러운 물리 계산)
    ////        rb.interpolation = RigidbodyInterpolation.Interpolate;
    ////    }

    ////    void FixedUpdate()
    ////    {
    ////        // 경사면을 감지하기 위한 Raycast
    ////        Debug.DrawLine(transform.position, Vector3.down * 2f, Color.red);

    ////        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
    ////        {
    ////            // 경사각 계산
    ////            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
    ////            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;

    ////            // 경사면을 따라 미끄러지는 방향 시각화
    ////            Debug.DrawRay(hit.point, hit.normal * 1f, Color.green);

    ////            // 경사각이 일정 이상이고, 속도가 낮으면 미끄러짐
    ////            if (slopeAngle > minSlideAngle && rb.linearVelocity.magnitude < speedLimit)
    ////            {
    ////                // 미끄러짐 강도 계산 (경사각에 비례)
    ////                float slideMultiplier = Mathf.Clamp(slopeAngle / 45f, 1f, maxSlideMultiplier);

    ////                // 미끄러짐 힘 계산
    ////                float slideForceToApply = slideForce * slideMultiplier;

    ////                // 자전거가 회전할 때에도 경사면에 맞게 미끄러짐을 유지
    ////                Vector3 slideDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;

    ////                // 미끄러짐 힘을 부드럽게 적용
    ////                rb.AddForce(slideDirection * slideForceToApply * Time.deltaTime, ForceMode.Force);
    ////            }
    ////        }

    ////        //자전거의 속도 제한(너무 빠르게 미끄러지지 않도록)
    ////        if (rb.linearVelocity.magnitude > speedLimit)
    ////        {
    ////            rb.linearVelocity = rb.linearVelocity.normalized * speedLimit;
    ////        }
    ////    }
    ////}




    void FixedUpdate()
    {
        Debug.Log("빙판길 올라옴");

        Debug.DrawLine(transform.position, transform.position + Vector3.down * 2f, Color.red);
        Debug.Log("차속력: " + carrb.linearVelocity.magnitude);
        Debug.Log("사람속력: " + Ragrb.linearVelocity.magnitude);


        //Debug.Log("Ray Start Position: " + (transform.position + Vector3.up * 1.3f));
        //if (Physics.Raycast(transform.position + Vector3.up * 1.3f, Vector3.down, out RaycastHit hit, 5f))
        //{
        //    Debug.Log("Ray Hit Point: " + hit.point);
        //    Debug.DrawRay(hit.point, slopeDirection * 2f, Color.blue);
        //}


        Debug.Log("Ray Start Position: " + (transform.position + Vector3.up * 1.3f));
        if (Physics.Raycast(transform.position + Vector3.up * 1.3f, Vector3.down, out RaycastHit hit))
        {
            Debug.Log("빙판길 올라옴2");

            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
            //Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.forward, hit.normal).normalized;


            Debug.Log("Ray Hit Point: " + hit.point);

            Debug.DrawRay(hit.point, hit.normal * 10f, Color.green); // 경사면의 법선 벡터를 시각적으로 확인

            Debug.DrawRay(hit.point, slopeDirection * 10f, Color.blue);

            Debug.Log("빙판각도" + slopeAngle);
            Debug.DrawRay(hit.point, hit.normal * 1f, Color.yellow);

            //  **내리막길 (미끄러지기)**
            if (slopeAngle > minSlideAngle)
            {
                float slideMultiplier = Mathf.Clamp(slopeAngle / 30f, 1f, maxSlideMultiplier); // 경사각에 따라 힘 증가
                carrb.AddForce(slopeDirection * (slideForce * slideMultiplier), ForceMode.Acceleration);

                // 내리막길에서 가속되도록 속도를 증가시킴
                carrb.linearVelocity += slopeDirection * Time.fixedDeltaTime * 5f; // 자연스럽게 더 빨라짐


                // 최대 속도 제한
                float maxSlideSpeed = 20f;
                if (carrb.linearVelocity.magnitude > maxSlideSpeed)
                {
                    carrb.linearVelocity = carrb.linearVelocity.normalized * maxSlideSpeed;
                }
            }

            //  **오르막길 (속도 유지 어려움)**
            if (slopeAngle > minSlideAngle && Vector3.Dot(carrb.linearVelocity.normalized, slopeDirection) < 0)
            {
                carrb.linearVelocity *= 0.98f; // 오르막길에서는 속도가 자연스럽게 줄어듦
            }
        }
        else
        {
            Debug.Log("Ray 미히트! 땅을 찾지 못함!");
        }
    }
}
