using UnityEngine;

public class RGTTire : MonoBehaviour
{

    [SerializeField] private GameObject TireRagRemy;
    [SerializeField] private GameObject Tire_1;
    [SerializeField] private GameObject RemyOri;


    [SerializeField] private Rigidbody Object;

    [SerializeField] private Transform Neck;
    [SerializeField] private Transform NeckPos;

    //private Transform OriginNeck;
    private Vector3 originNeckV2;

    private void Start()
    {
        //OriginNeck.transform.position = Neck.transform.position;
        originNeckV2 = Neck.transform.position;
    }

    private void Update()
    {
        RandomValue();
        //Checkposition();
        FollowThePos();
        Rotatey(1*8);
        Checkposition();

        //Debug.Log("z: " + transform.localPosition.z);

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            //transform.eulerAngles += Vector3.forward * balanceSpeed * Time.deltaTime;
            //transform.Rotate(0f,0f,+RotationValue);
            //animator.SetBool("SetLeftActive", false);
            //RotateZ(1);
            Posx(-1);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //animator.SetBool("SetRightActive", false);
            //transform.eulerAngles += Vector3.back * balanceSpeed * Time.deltaTime;
            //RotateZ(-1);
            Posx(1);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.eulerAngles += Vector3.right * balanceSpeed * Time.deltaTime;
            //animator.SetBool("SetUpActive", false);
            //RotateX(1);
            Posz(1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.eulerAngles += Vector3.up * balanceSpeed * Time.deltaTime;
            //animator.SetBool("SetBackActive", false);
            //RotateX(-1);
            Posz(-1);
        }
    }

    private void Rotatey(float _direction)
    {
        //float zRotation = _direction * balanceSpeed * Time.deltaTime;
        ////transform.Rotate( 0f, 0f, zRotation, Space.Self);
        //float newZRotation = transform.eulerAngles.z + zRotation;

        //// 유니티의 EulerAngles는 0~360도로 표현되므로 -60~60도로 변환해야 함
        //if (newZRotation > 180f)
        //    newZRotation -= 360f;

        //newZRotation = Mathf.Clamp(newZRotation, -60f, 60f);

        //transform.rotation = Quaternion.Euler(0f, 0f, newZRotation);
        transform.Rotate(Vector3.up, _direction * Time.deltaTime * 200f);
    }

    private void Posx(float _dir)
    {
        //transform.Translate(Vector3.right * _dir* Time.deltaTime);
        transform.Translate(Vector3.right * _dir * Time.deltaTime * 1f, Space.World);

    }    
    private void Posz(float _dir)
    {
        //transform.Translate(Vector3.right * _dir* Time.deltaTime);
        transform.Translate(Vector3.forward * _dir * Time.deltaTime * 1f, Space.World);

    }

    private void RandomValue()
    {
        int a = Random.Range(1, 4);
        if(a == 1)
        {
            Posx(-1);
        }
        else if (a == 2)
        {
            Posx(1);
        }
        else if(a == 3)
        {
            Posz(-1);
        }
        else if(a == 4)
        {
            Posz(1);
        }
    }

    private void FollowThePos()
    {
        Neck.transform.position = NeckPos.transform.position;
    }

    private void Checkposition()
    {
        Vector3 Offset = Neck.transform.position - originNeckV2;
        float Length = Offset.magnitude;
        Debug.Log("Length : " + Length);

        float Limit = 0.5f;

        if(Length >= Limit)
        {
            //

            TireRagRemy.SetActive(true);
            Tire_1.SetActive(true);
            Destroy(RemyOri);
            this.enabled = false;

        }

        
    }




}
