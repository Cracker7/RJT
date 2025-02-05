using UnityEngine;

public class ArrowInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        float isAKeyPressed = Input.GetAxis("Horizontal");

        // 화살표 키가 눌렸을 경우 값을 0으로 만들기
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            isAKeyPressed = 0f;
        }

        return new Vector3(isAKeyPressed, 0, 0);
    }
}
