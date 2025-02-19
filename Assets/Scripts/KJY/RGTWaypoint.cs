using UnityEngine;
using UnityEngine.AI;

public class RGTWaypoint : MonoBehaviour
{
    [SerializeField] private Transform originTrs; // 사용할 위치 배열


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            other.gameObject.transform.position = originTrs.position;
        }
    }


}
