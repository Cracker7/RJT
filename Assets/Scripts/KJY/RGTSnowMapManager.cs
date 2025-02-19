using UnityEngine;

public class RGTSnowMapManager : MonoBehaviour
{
    [SerializeField] private GameObject ForestMap;
    [SerializeField] private GameObject DesertMap;
    [SerializeField] private GameObject SnowMap;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            ForestMap.SetActive(false);
            DesertMap.SetActive(false);
            SnowMap.SetActive(true);
        }
    }



}
