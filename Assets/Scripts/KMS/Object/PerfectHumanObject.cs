using UnityEngine;

public class PerfectHumanObject : InteractableObject
{
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        AudioManager.instance.PlaySfx(AudioManager.sfx.human);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        AudioManager.instance.PlaySfx(AudioManager.sfx.human);
    }
}
