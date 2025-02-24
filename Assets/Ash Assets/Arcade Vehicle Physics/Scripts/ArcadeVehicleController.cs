using UnityEngine;

namespace ArcadeVP
{
    public class ArcadeVehicleController : MonoBehaviour, IMovement
    {
        public enum groundCheck { rayCast, sphereCaste };
        public enum MovementMode { Velocity, AngularVelocity };
        public MovementMode movementMode;
        public groundCheck GroundCheck;
        public LayerMask drivableSurface;

        public float MaxSpeed, accelaration, turn, gravity = 7f, downforce = 5f;
        [Tooltip("공중 컨트롤 가능 여부")]
        public bool AirControl = false;
        [Tooltip("추가적인 공중 이동 힘. 값이 클수록 공중에서 이동 제어가 강해집니다.")]
        public float airControlTranslationForce = 50f;
        [Tooltip("스페이스바를 누르면 멈추는 대신 드리프트가 됨")]
        public bool kartLike = false;
        [Tooltip("kartLike가 true일 경우 드리프트하는 동안 값에 따라 더 많이 회전할 수 있음")]
        public float driftMultiplier = 1.5f;

        [Header("Speed Limit Settings")]
        [Tooltip("최대속도를 초과했을 때, 서서히 최대속도로 감속하는 정도를 결정합니다.")]
        public float decelerationFactor = 10f;

        public Rigidbody rb, carBody;

        [HideInInspector]
        public RaycastHit hit;
        public AnimationCurve frictionCurve;
        public AnimationCurve turnCurve;
        public PhysicsMaterial frictionMaterial;
        [Header("Visuals")]
        public Transform BodyMesh;
        public Transform[] FrontWheels = new Transform[2];
        public Transform[] RearWheels = new Transform[2];
        [HideInInspector]
        public Vector3 carVelocity;

        [Range(0, 10)]
        public float BodyTilt;
        [Header("Audio settings")]
        public AudioSource engineSound;
        [Range(0, 1)]
        public float minPitch;
        [Range(1, 3)]
        public float MaxPitch;
        public AudioSource SkidSound;

        [HideInInspector]
        public float skidWidth;

        public bool parallelMovement = false;

        private float radius, steeringInput, accelerationInput, brakeInput;
        private Vector3 origin;

        public ICarDown carDown;

        public bool inDesert = false;
        public bool InDesert { get { return inDesert; } set { inDesert = value; } }

        public Transform bodyTr;
        //public Transform BodyTr { get { return bodyTr; } set { bodyTr = value; } }

        private void Start()
        {
            radius = rb.GetComponent<SphereCollider>().radius;
            if (movementMode == MovementMode.AngularVelocity)
            {
                Physics.defaultMaxAngularSpeed = MaxSpeed;
            }
            carDown = new RGTCarDownV2();
        }

        private void Update()
        {
            Visuals();
            AudioManager();
        }

        //public void ProvideInputs(float _steeringInput, float _accelarationInput, float _brakeInput)
        //{
        //    steeringInput = _steeringInput;
        //    accelerationInput = _accelarationInput;
        //    brakeInput = _brakeInput;
        //}

        public void AudioManager()
        {
            engineSound.pitch = Mathf.Lerp(minPitch, MaxPitch, Mathf.Abs(carVelocity.z) / MaxSpeed);
            if (Mathf.Abs(carVelocity.x) > 10 && grounded())
            {
                SkidSound.mute = false;
            }
            else
            {
                SkidSound.mute = true;
            }
        }

        public void Move(Vector3 input)
        {
            steeringInput = input.x;
            brakeInput = input.y;
            //accelerationInput = input.z;
            // 자동으로 계속해서 이동
            accelerationInput = 1f;


            carVelocity = carBody.transform.InverseTransformDirection(carBody.linearVelocity);

            if (Mathf.Abs(carVelocity.x) > 0)
            {
                //changes friction according to sideways speed of car
                frictionMaterial.dynamicFriction = frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100));
                //frictionMaterial.dynamicFriction = Mathf.Clamp(frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100)), 0.1f, 1f);
            }


            if (grounded())
            {
                //turnlogic
                float sign = Mathf.Sign(carVelocity.z);
                //float sign = 1f;
                float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);
                if (kartLike && brakeInput > 0.1f) { TurnMultiplyer *= driftMultiplier; } //turn more if drifting

                if (!parallelMovement)
                {
                    //if (accelerationInput > 0.1f || carVelocity.z > 1)
                    //{
                    //    carBody.AddTorque(Vector3.up * steeringInput * sign * turn * 100 * TurnMultiplyer);
                    //}
                    //else if (accelerationInput < -0.1f || carVelocity.z < -1)
                    //{
                    //    carBody.AddTorque(Vector3.up * steeringInput * sign * turn * 100 * TurnMultiplyer);
                    //}
                    // 항상 회전력을 가하도록 수정
                    carBody.AddTorque(Vector3.up * steeringInput * sign * turn * 100 * TurnMultiplyer);
                }
                else
                {
                    if (accelerationInput > 0.1f || carVelocity.z > 1 || accelerationInput < -0.1f || carVelocity.z < -1)
                    {
                        Vector3 lateralMovement = transform.right * steeringInput * turn * 100;
                        carBody.AddForce(lateralMovement, ForceMode.Acceleration);
                    }
                }



                // mormal brakelogic
                if (!kartLike)
                {
                    if (brakeInput > 0.1f)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeRotationX;
                    }
                    else
                    {
                        rb.constraints = RigidbodyConstraints.None;
                    }
                }

                //accelaration logic

                if (movementMode == MovementMode.AngularVelocity)
                {
                    if (Mathf.Abs(accelerationInput) > 0.1f && brakeInput < 0.1f && !kartLike)
                    {
                        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * accelerationInput * MaxSpeed / radius, accelaration * Time.fixedDeltaTime);
                    }
                    else if (Mathf.Abs(accelerationInput) > 0.1f && kartLike)
                    {
                        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, carBody.transform.right * accelerationInput * MaxSpeed / radius, accelaration * Time.fixedDeltaTime);
                    }
                }
                else if (movementMode == MovementMode.Velocity)
                {
                    if (Mathf.Abs(accelerationInput) > 0.1f && brakeInput < 0.1f && !kartLike)
                    {
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, carBody.transform.forward * accelerationInput * MaxSpeed, accelaration / 10 * Time.fixedDeltaTime);
                    }
                    else if (Mathf.Abs(accelerationInput) > 0.1f && kartLike)
                    {
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, carBody.transform.forward * accelerationInput * MaxSpeed, accelaration / 10 * Time.fixedDeltaTime);
                    }
                }

                //// 트리거가 작동할 경우 carDown을 실행시키는 코드
                //if (inDesert && carBody.linearVelocity.magnitude <= 60)
                //{
                //    Debug.Log("빠지기 실행됨 : " + bodyTr);
                //    carDown.Sink(bodyTr);
                //}

                if(inDesert)
                {
                    if(carBody.linearVelocity.magnitude <= 60)
                    {
                        carDown.Sink(bodyTr);
                    }
                    carDown.Rising(bodyTr);
                }


                // down froce
                rb.AddForce(-transform.up * downforce * rb.mass);

                //body tilt
                carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, hit.normal) * carBody.transform.rotation, 0.12f));
            }
            else
            {
                // if (AirControl)
                // {
                //     //turnlogic
                //     float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);

                //     carBody.AddTorque(Vector3.up * steeringInput * turn * 100 * TurnMultiplyer);
                // }

                // carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation, 0.02f));
                // rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, rb.linearVelocity + Vector3.down * gravity, Time.deltaTime * gravity);

                // 공중일 때 (AirControl 활성화 시 좌우 회전 + 이동 제어)
                if (AirControl)
                {
                    // 기존 좌우 회전 토크 적용
                    float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / MaxSpeed);
                    carBody.AddTorque(Vector3.up * steeringInput * turn * airControlTranslationForce * TurnMultiplyer);

                    // 새롭게 공중 이동 힘 추가 (전진/후진 및 약간의 측면 이동)
                    Vector3 airTranslationForce = (carBody.transform.right * steeringInput) * airControlTranslationForce;
                    carBody.AddForce(airTranslationForce, ForceMode.Acceleration);

                    //if (!grounded())
                    //{
                    //    // 차량이 표면에 밀착되도록 추가 힘 적용
                    //    rb.AddForce(-transform.up * gravity * rb.mass);
                    //}
                }

                // 공중에서는 차량이 천천히 원래 자세(수직)로 돌아오도록 회전 보정
                carBody.MoveRotation(Quaternion.Slerp(carBody.rotation,
                    Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation,
                    0.02f));

                // 중력 적용
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, rb.linearVelocity + Vector3.down * gravity, Time.fixedDeltaTime * gravity);

                // **속도가 너무 빠르면 최대속도에 맞춰 서서히 감속**
                if (rb.linearVelocity.magnitude > MaxSpeed)
                {
                    rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, rb.linearVelocity.normalized * MaxSpeed, decelerationFactor * Time.fixedDeltaTime);
                }
            }

        }
        public void Visuals()
        {
            //tires
            foreach (Transform FW in FrontWheels)
            {
                FW.localRotation = Quaternion.Slerp(FW.localRotation, Quaternion.Euler(FW.localRotation.eulerAngles.x,
                                   30 * steeringInput, FW.localRotation.eulerAngles.z), 0.7f * Time.deltaTime / Time.fixedDeltaTime);
                FW.GetChild(0).localRotation = rb.transform.localRotation;
            }
            RearWheels[0].localRotation = rb.transform.localRotation;
            RearWheels[1].localRotation = rb.transform.localRotation;

            //Body
            if (carVelocity.z > 1)
            {
                BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(Mathf.Lerp(0, -5, carVelocity.z / MaxSpeed),
                                   BodyMesh.localRotation.eulerAngles.y, BodyTilt * steeringInput), 0.4f * Time.deltaTime / Time.fixedDeltaTime);
            }
            else
            {
                BodyMesh.localRotation = Quaternion.Slerp(BodyMesh.localRotation, Quaternion.Euler(0, 0, 0), 0.4f * Time.deltaTime / Time.fixedDeltaTime);
            }


            if (kartLike)
            {
                if (brakeInput > 0.1f)
                {
                    BodyMesh.parent.localRotation = Quaternion.Slerp(BodyMesh.parent.localRotation,
                    Quaternion.Euler(0, 45 * steeringInput * Mathf.Sign(carVelocity.z), 0),
                    0.1f * Time.deltaTime / Time.fixedDeltaTime);
                }
                else
                {
                    BodyMesh.parent.localRotation = Quaternion.Slerp(BodyMesh.parent.localRotation,
                    Quaternion.Euler(0, 0, 0),
                    0.1f * Time.deltaTime / Time.fixedDeltaTime);
                }

            }

        }

        public bool grounded() //checks for if vehicle is grounded or not
        {
            //origin = rb.position + rb.GetComponent<SphereCollider>().radius * Vector3.up;
            origin = rb.position + transform.up * (rb.GetComponent<SphereCollider>().radius + 0.1f);
            var direction = -transform.up;
            var maxdistance = rb.GetComponent<SphereCollider>().radius + 5f;

            if (GroundCheck == groundCheck.rayCast)
            {
                if (Physics.Raycast(rb.position, Vector3.down, out hit, maxdistance, drivableSurface))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            else if (GroundCheck == groundCheck.sphereCaste)
            {
                if (Physics.SphereCast(origin, radius * 0.9f, direction, out hit, maxdistance, drivableSurface))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 벽으로 태그된 오브젝트와 충돌 시
            if (collision.gameObject.CompareTag("Wall"))
            {
                Vector3 averageNormal = Vector3.zero;
                foreach (ContactPoint contact in collision.contacts)
                {
                    averageNormal += contact.normal;
                }
                averageNormal.Normalize();

                // 충돌 법선 방향으로 반동 힘을 적용합니다.
                float reboundForce = 100f; // 이 값을 조절하여 반동 강도를 결정합니다.
                rb.AddForce(averageNormal * reboundForce, ForceMode.Impulse);

                // 벽의 로컬 Z방향(즉, wall.transform.forward)으로 회전 토크를 추가합니다.
                Vector3 wallZDirection = collision.gameObject.transform.forward;
                carBody.transform.rotation = Quaternion.Euler(wallZDirection);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Vector3 averageNormal = Vector3.zero;
                foreach (ContactPoint contact in collision.contacts)
                {
                    averageNormal += contact.normal;
                }
                averageNormal.Normalize();

                // 차량의 진행 방향과 벽의 반대 방향(-법선) 사이의 각도를 계산합니다.
                float angleDiff = Vector3.SignedAngle(carBody.transform.forward, -averageNormal, Vector3.up);

                // 이 각도에 비례하는 회전 토크를 추가하여 좌우 회전을 유도합니다.
                float rotationTorque = angleDiff * 5f; // 계수를 조절하여 회전 강도를 결정합니다.
                carBody.AddTorque(Vector3.up * rotationTorque, ForceMode.Acceleration);
            }
        }

        private void OnDrawGizmos()
        {
            //debug gizmos
            radius = rb.GetComponent<SphereCollider>().radius;
            float width = 0.02f;
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(rb.transform.position + ((radius + width) * Vector3.down), new Vector3(2 * radius, 2 * width, 4 * radius));
                if (GetComponent<BoxCollider>())
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
                }

            }

        }
    }
}
