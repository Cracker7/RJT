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
            Debug.Log("km/h : " + rb.linearVelocity.magnitude * 3.6f);
            //Debug.Log("°¢ ¼Óµµ : " + rb.angularVelocity.magnitude);
            time = 0;
        }
    }
}
