using UnityEngine;

public class ArrowKey : MonoBehaviour
{

    [SerializeField] private float balanceSpeed = 100f;

    //private float RotationValue = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.eulerAngles += Vector3.forward * balanceSpeed * Time.deltaTime;
            //transform.Rotate(0f,0f,+RotationValue);
            RotateZ(1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //transform.eulerAngles += Vector3.back * balanceSpeed * Time.deltaTime;
            RotateZ(-1);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.eulerAngles += Vector3.right * balanceSpeed * Time.deltaTime;
            RotateX(1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.eulerAngles += Vector3.up * balanceSpeed * Time.deltaTime;
            RotateX(-1);
        }
        else
        {
            // RanDomRotation();
        }
    }

    private void RotateX(float _direction)
    {
        float xRotation = _direction * balanceSpeed * Time.deltaTime;
        //transform.Rotate(xRotation, 0f, 0f, Space.Self);

        float newXRotation = transform.eulerAngles.x + xRotation;

        // 유니티의 EulerAngles는 0~360도로 표현되므로 -60~60도로 변환해야 함
        if (newXRotation > 180f)
            newXRotation -= 360f;

        newXRotation = Mathf.Clamp(newXRotation, -60f, 60f);

        transform.rotation = Quaternion.Euler(newXRotation, 0f, 0f);
    }    

    private void RotateZ(float _direction)
    {
        float zRotation = _direction * balanceSpeed * Time.deltaTime;
        //transform.Rotate( 0f, 0f, zRotation, Space.Self);
        float newZRotation = transform.eulerAngles.z + zRotation;

        // 유니티의 EulerAngles는 0~360도로 표현되므로 -60~60도로 변환해야 함
        if (newZRotation > 180f)
            newZRotation -= 360f;

        newZRotation = Mathf.Clamp(newZRotation, -60f, 60f);

        transform.rotation = Quaternion.Euler(0f, 0f, newZRotation);
    }

    private void RanDomRotation()
    {
        //transform.Translate(new Vector3(Random.Range(0f, 1.3f),0f, Random.Range(0f, 1.3f)));
        //transform.rotation = transform.localRotation(Quaternion.Euler(Random.Range(1f, 90f), Random.Range(1f, 90f), Random.Range(1f, 90f)));

        float randomRotation = Random.Range(-90f, 90f) * Time.deltaTime;
        transform.Rotate(0, randomRotation, randomRotation);

    }
}
