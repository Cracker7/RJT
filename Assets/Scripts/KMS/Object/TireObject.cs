using UnityEngine;

public class TireObject : InteractableObject
{
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.instance.PlaySfx(AudioManager.sfx.tire);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        AudioManager.instance.PlaySfx(AudioManager.sfx.tire);
    }
}
