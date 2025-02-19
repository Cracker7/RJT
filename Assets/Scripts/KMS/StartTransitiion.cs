using Unity.VisualScripting;
using UnityEngine;

public class StartTransitiion : MonoBehaviour
{
    public PlayerKMS playerKMS;
    public InteractableObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            playerKMS.StartTransition(target);
        }
    }
}
