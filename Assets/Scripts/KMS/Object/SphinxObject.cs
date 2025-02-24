using UnityEngine;

public class SphinxObject : InteractableObject
{
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.instance.PlaySfx(AudioManager.sfx.sphinx);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        AudioManager.instance.PlaySfx(AudioManager.sfx.sphinx);
    }
}
