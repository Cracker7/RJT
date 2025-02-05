using UnityEngine;
using UnityEngine.Purchasing;

public class SphereObject : InteractableObject
{
    [SerializeField] private Vector3 velocity;           // 현재 속도
    [SerializeField] private float forceValue = 10f;
    
    public float durabilityLossRate = 0.1f;
    public bool RockActive = false;

    void Start()
    {
        velocity = Vector3.zero; // 초기 속도 설정
    }

    private void Update()
    {
        // 힘을 적용하여 속도 증가
        velocity += Vector3.forward * forceValue * Time.deltaTime;

        if (RockActive == true)
        {
            Mdurability();
            //Debug.Log("currentDurability"+ currentDurability);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            RockActive = true;
        }
    }

    private void Mdurability()
    {

        //Debug.Log("Here : " + velocity.magnitude);
        // 움직이는 동안 내구도 감소
        if (velocity.magnitude > 0.1f)
        {
            currentDurability -= durabilityLossRate * Time.deltaTime;
        }
        if(currentDurability <= 0)
        {
            RockActive = false;
            Debug.Log("false");
        }
    }

}
