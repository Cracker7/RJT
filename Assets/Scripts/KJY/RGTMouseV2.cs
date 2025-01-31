using UnityEngine;

public class RGTMouseV2 : MonoBehaviour
{

    //물체 자연스럽게 앞으로 이동하고 있고, 마우스 클릭했을 때 물체의 X좌표는 마우스의 X좌표를 따라간다.
    //힘을 이용하여 이동해야 한다.


    private bool isDragging = false;


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
                else
                {
                    isDragging = false;
                }
            }
        }

    }

    public void FallowThePos(GameObject _object)
    {
        Rigidbody rb = _object.GetComponent<Rigidbody>();
        GameObject Object = _object;

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

    

}
