using UnityEngine;

public class MountableVehicle : MonoBehaviour
{
    public MountableVehicleData vehicleData;
    public Transform mountPoint;
    private IMovement movement;
    private IInputHandler input;

    void Awake()
    {
        movement = GetComponent<IMovement>();
        input = GetComponent<IInputHandler>();
    }
}
