using UnityEngine;

public class ADInputHandler : MonoBehaviour, IInputHandler
{
    public Transform camTr;
    public float HandleInput()
    {
        Debug.Log("AD키 입력 받는중");

        float isAKeyPressed = Input.GetAxis("Horizontal");

        // 화살표 키가 눌렸을 경우 값을 0으로 만들기
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isAKeyPressed = 0f;
        }

        return isAKeyPressed;

    }
}
