using UnityEngine;

public class WheelController : MonoBehaviour,IMovement
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    private Rigidbody rb;
    private float time;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private float maxSpeed = 60f;
    [SerializeField] private float inputXSet = 3f;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 input)
    {
        horizontalInput = input.x / inputXSet;
        verticalInput = input.z;
        HandleMotor();
        HandleSteering();
    }
    //private void FixedUpdate()
    //{
    //    GetInput();
    //    HandleMotor();
    //    HandleSteering();
    //    UpdateWheels();
    //}

    //private void GetInput()
    //{
    //    // Steering Input
    //    horizontalInput = Input.GetAxis("Horizontal")/2;
    //    horizontalInput = 

    //    // Acceleration Input
    //    verticalInput = Input.GetAxis("Vertical");

    //    // Breaking Input
    //    isBreaking = Input.GetKey(KeyCode.Space);
    //}

    private void HandleMotor()
    {
        if (rb.linearVelocity.magnitude < maxSpeed) // 현재 속도가 최대 속력보다 작을 때만 가속
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        //else
        //{
        //    //frontLeftWheelCollider.motorTorque = 0; // 최대 속도 도달 시 추가적인 힘을 주지 않음
        //    //frontRightWheelCollider.motorTorque = 0;
        //    rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        //}
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
