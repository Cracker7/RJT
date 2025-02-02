using UnityEngine;

public class RGTTEst : MonoBehaviour
{
    public Rigidbody boxRigidbody; 
    public float gravityForce = 50f; 
    public float lateralForce = 10f; 
    public float maxSpeed = 10f;

    void Start()
    {
        boxRigidbody.useGravity = false; 
    }

    void FixedUpdate()
    {
        Vector3 customGravity = new Vector3(0, -1, -1).normalized * gravityForce;
        boxRigidbody.AddForce(customGravity, ForceMode.Acceleration);

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 lateralMovement = Vector3.right * horizontalInput * lateralForce;

        boxRigidbody.AddForce(lateralMovement, ForceMode.Force);

        LimitSpeed();
    }

    private void LimitSpeed()
    {
        if (boxRigidbody.linearVelocity.magnitude > maxSpeed)
        {
            boxRigidbody.linearVelocity = boxRigidbody.linearVelocity.normalized * maxSpeed;
        }
    }
}