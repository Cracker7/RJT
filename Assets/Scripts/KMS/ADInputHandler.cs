using UnityEngine;

public class ADInputHandler : IInputHandler
{
    private float _moveSpeed = 5f;
    public ADInputHandler(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public Vector3 HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // A, D 키 입력값을 -1 ~ 1 사이의 값으로 반환

        return new Vector2(horizontalInput, 0);
    }
}
