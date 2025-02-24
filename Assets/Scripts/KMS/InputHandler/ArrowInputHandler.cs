using UnityEngine;

public class ArrowInputHandler : MonoBehaviour, IInputHandler
{
    public InputType Type => InputType.Arrow;
    public Vector3 HandleInput()
    {
        Debug.Log("Arrow Input");

        float XAxis = Input.GetAxis("Horizontal");
        float YAxis = Input.GetAxis("Jump");

        // AD 키가 눌렸을 경우 값을 0으로 만들기
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            XAxis = 0;
        }

        return new Vector3(XAxis, YAxis, 0);
    }
}
