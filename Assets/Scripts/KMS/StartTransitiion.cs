using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class StartTransitiion : MonoBehaviour
{
    public PlayerKMS playerKMS;
    public InteractableObject target;

    public CinemachineFollow followCam;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            playerKMS.StartTransition(target);
        }

        followCam.FollowOffset += new Vector3(0, -4, 0);
    }
}
