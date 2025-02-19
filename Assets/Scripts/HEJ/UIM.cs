using UnityEngine;
using TMPro;

public class UIM : MonoBehaviour
{
    public Transform player;      // 플레이어 위치
    public Vector3 offset;        // 플레이어와의 거리 조정
    public TextMeshProUGUI text;  // 대사 표시

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;  // 플레이어 옆에 위치
            transform.LookAt(Camera.main.transform);       // 카메라를 향하도록 회전
        }
    }

   
}
