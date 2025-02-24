using UnityEngine;

public class CarObject : InteractableObject
{
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        // 오브젝트에 맞는 소리 나는것 적용

    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        // 오브젝트에 맞는 소리 나는것 적용
    }
}
