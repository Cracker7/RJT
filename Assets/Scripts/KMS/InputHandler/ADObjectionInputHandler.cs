using UnityEngine;

public class ADObjectionInputHandler : MonoBehaviour, IInputHandler
{
    public float HandleInput()
    {
        Debug.Log("반대 AD키 입력 받는중");
        // A 키와 D 키 입력 받기
        float isAKeyPressed = Input.GetAxis("Horizontal");

        // 화살표 키가 눌렸을 경우 값을 0으로 만들기
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isAKeyPressed = 0f;
        }

        return -isAKeyPressed;

    }
}
