using UnityEngine;

public class OnRemy : MonoBehaviour
{
    public GameObject SitRemy;
    public GameObject RunRemy;


    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("플레이어 못 닿음");
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 닿음");
            RunRemy.SetActive(false);
            SitRemy.SetActive(true);
        }
    }

}
