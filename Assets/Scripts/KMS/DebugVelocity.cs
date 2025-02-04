using UnityEngine;

public class DebugVelocity : MonoBehaviour
{
    private float time;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        time += Time.deltaTime;

        if (time > 1f)
        {
            Debug.Log("리니어 속도 : " + rb.linearVelocity.magnitude);
            Debug.Log("각 속도 : " + rb.angularVelocity.magnitude);
            time = 0;
        }
    }
}
