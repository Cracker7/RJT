using UnityEngine;

public class HatObject : InteractableObject
{
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.instance.PlaySfx(AudioManager.sfx.hat);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        AudioManager.instance.PlaySfx(AudioManager.sfx.hat);
    }
}
