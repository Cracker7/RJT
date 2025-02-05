using UnityEngine;

public class AddForce : MonoBehaviour
{
    private Rigidbody rb;
    public Transform camTr;
    public float maxSpeed = 40f;
    public float angle = 4f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputZ = Input.GetAxis("Vertical");
        {
            Debug.Log("½ºÆäÀÌ½º ÀÔ·ÂµÊ");
            rb.AddForce(transform.forward * moveInputZ * angle, ForceMode.Acceleration);
            rb.AddForce(transform.right * moveInputX * angle, ForceMode.Acceleration);
            Debug.Log("Èû ÁÖ±â");
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
