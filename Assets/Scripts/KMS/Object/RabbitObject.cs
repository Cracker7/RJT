using UnityEngine;

public class RabbitObject : InteractableObject
{
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.instance.PlaySfx(AudioManager.sfx.rabbit);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        AudioManager.instance.PlaySfx(AudioManager.sfx.rabbit);
    }
}
