using UnityEngine;

public class ADInputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 HandleInput()
    {
        Debug.Log("AD키 입력 받는중");
        // A 키와 D 키 입력 받기
        bool isAKeyPressed = Input.GetKey(KeyCode.A);
        bool isDKeyPressed = Input.GetKey(KeyCode.D);

        if (isAKeyPressed)
        {
            return -transform.right;
        }
        else if (isDKeyPressed)
        {
            return transform.right;
        }
        return Vector3.zero;

    }
}
