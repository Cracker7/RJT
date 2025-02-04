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
        float moveInput = Input.GetAxis("Horizontal");
        {
            Debug.Log("½ºÆäÀÌ½º ÀÔ·ÂµÊ");
            rb.AddForce(camTr.right * moveInput * angle, ForceMode.Acceleration);
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
