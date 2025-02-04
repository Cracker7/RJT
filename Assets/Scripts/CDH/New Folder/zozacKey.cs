using UnityEngine;

public class zozacKey : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 5f;
    public float mass = 1f;

    public Rigidbody rb;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        rb.mass = mass;
    }

    void FixedUpdate()
    {
        // 전진 이동
       // rb.AddForce(transform.forward * speed);

        // 회전 입력
        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(Vector3.up * turn * turnSpeed);
    }
}
