using UnityEngine;

public class ArrowInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        // 화살표 키 입력에 따른 이동 방향 벡터 초기화
        Vector3 input = Vector3.zero;

        // 좌우 입력 처리
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            input.x = -1;  // 왼쪽 이동
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            input.x = 1;   // 오른쪽 이동
        }

        input = input.normalized;  // 입력 벡터 정규화

        return input;
    }
}
