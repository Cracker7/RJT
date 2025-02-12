using UnityEngine;

public class CircleObject : InteractableObject
{
    public Transform tr;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //rb.AddTorque(transform.forward*10);
        tr.localRotation *= Quaternion.Euler(transform.right * 10);

    }
}
