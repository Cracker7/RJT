using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ADK : MonoBehaviour
{

    public Rigidbody rollingRigidbody; // 물체의 Rigidbody
    public float torqueForce = 10f; // 좌우 회전 힘
    public float forwardForce = 15f; // 전진 힘
    public float maxAngularVelocity = 10f; // 최대 회전 속도 제한

    void Start()
    {
        // 최대 각속도 설정 (너무 빠르게 회전하지 않도록 제한)
        rollingRigidbody.maxAngularVelocity = maxAngularVelocity;
    }

    void FixedUpdate()
    {
        // A 키와 D 키 입력 받기
        bool isAKeyPressed = Input.GetKey(KeyCode.A);
        bool isDKeyPressed = Input.GetKey(KeyCode.D);

        if (isAKeyPressed)
        {
            // A 키를 누를 때 왼쪽으로 힘과 토크 적용
            rollingRigidbody.AddForce(Vector3.left * forwardForce, ForceMode.Force);
            rollingRigidbody.AddTorque(Vector3.up * -torqueForce, ForceMode.Force);
        }
        else if (isDKeyPressed)
        {
            // D 키를 누를 때 오른쪽으로 힘과 토크 적용
            rollingRigidbody.AddForce(Vector3.right * forwardForce, ForceMode.Force);
            rollingRigidbody.AddTorque(Vector3.up * torqueForce, ForceMode.Force);
        }
        else
        {
            // 키 입력 없을 때 각속도 감속
            rollingRigidbody.angularVelocity *= 0.95f;
        }
    }


}




