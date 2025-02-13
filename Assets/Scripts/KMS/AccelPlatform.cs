using UnityEngine;

public class AccelPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        // 레이어로 필터링
        if (collider.gameObject.layer == LayerMask.NameToLayer("SphereRB"))
        {
            Debug.Log("발판 밟음");
            // 충돌한 오브젝트의 Rigidbody 컴포넌트를 가져옴
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            // Rigidbody 컴포넌트가 없다면 리턴
            if (rb == null)
            {
                return;
            }
            // Rigidbody 컴포넌트에 힘을 가함
            rb.AddForce(transform.forward * 200f, ForceMode.Impulse);
        }

    }
}
