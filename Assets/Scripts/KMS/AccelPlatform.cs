using UnityEngine;

public class AccelPlatform : MonoBehaviour
{

    public float speed = 500f;

    private void OnTriggerEnter(Collider collider)
    {
        // ���̾�� ���͸�
        if (collider.gameObject.layer == LayerMask.NameToLayer("SphereRB"))
        {
            Debug.Log("���� ����");
            // �浹�� ������Ʈ�� Rigidbody ������Ʈ�� ������
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            // Rigidbody ������Ʈ�� ���ٸ� ����
            if (rb == null)
            {
                return;
            }
            // Rigidbody ������Ʈ�� ���� ����
            //rb.AddForce(transform.forward * 200f, ForceMode.Impulse);
            rb.linearVelocity *= speed;
        }

    }
}
