using UnityEngine;

public class RGTMouseV2 : MonoBehaviour
{

    //물체 자연스럽게 앞으로 이동하고 있고, 마우스 클릭했을 때 물체의 X좌표는 마우스의 X좌표를 따라간다.
    //힘을 이용하여 이동해야 한다.


    public GameObject hat;
    private bool isDragging = false;
    public float torqueForce = 10f; // 좌우 회전 힘
    public float forwardForce = 15f; // 전진 힘

    [SerializeField] private Rigidbody[] ragdollLimbs;


    private void Update()
    {
        FallowThePos(hat);
    }

    private void GetMouseButton()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "hat")
                {
                    isDragging = true;
                }

            }
        }
        else
        {
            isDragging = false;
        }

    }

    public void FallowThePos(GameObject _object)
    {
        Rigidbody rb = _object.GetComponent<Rigidbody>();
        GameObject Object = _object;

        rb.AddForce(Vector3.forward * forwardForce, ForceMode.Force);
        rb.AddTorque(Vector3.up * -torqueForce, ForceMode.Force);
        GetMouseButton();

        if(isDragging)
        {
            //모자의 x좌표, 마우스의 x좌표 따라 간다.

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Vector3 newPosition = Object.transform.position;
                newPosition.x = hit.point.x;
                Object.transform.position = newPosition;
            }
        }
    }

    //private void FlapRagdoll(Vector3 direction)
    //{
    //    foreach (Rigidbody limb in ragdollLimbs)
    //    {
    //        Vector3 randomForce = direction * -1 * ragdollFlapForce * Random.Range(0.8f, 1.2f);
    //        limb.AddForce(randomForce, ForceMode.Impulse);  // 랜덤한 힘을 추가
    //    }
    //}

}
