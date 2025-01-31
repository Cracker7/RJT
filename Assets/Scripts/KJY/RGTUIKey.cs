using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class RGTUIKey : MonoBehaviour
{
    //이동속도
    [SerializeField] private float MoveSpeed = 50f;
    //부드러운 이동시간
    [SerializeField] private float SmoothTime = 0.1f;

    //목표위치
    private Vector3 TargetPosition;
    //현재속도
    private Vector3 velocity = Vector3.zero;


    public void UIKey(GameObject _object)
    {
        TargetPosition = _object.transform.position;
        GameObject Object = _object;

        if (Input.GetMouseButton(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition; // 마우스 위치

            // Raycast 결과를 저장할 리스트
            System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            // 결과 처리
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Left")) // 태그가 "Left"인 UI 요소
                {
                    Debug.Log("Left 클릭");
                    TargetPosition += Vector3.left * MoveSpeed * Time.deltaTime;
                }
                else if (result.gameObject.CompareTag("Right")) // 태그가 "Right"인 UI 요소
                {
                    Debug.Log("Right 클릭");
                    TargetPosition += Vector3.right * MoveSpeed * Time.deltaTime;
                }
            }
                Object.transform.position = Vector3.SmoothDamp(Object.transform.position, TargetPosition, ref velocity, SmoothTime);
        }
    }


}
