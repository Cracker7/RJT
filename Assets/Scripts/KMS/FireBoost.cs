using ArcadeVP;
using UnityEngine;

public class FireBoost : MonoBehaviour
{
    private ParticleSystem fire;
    public ArcadeVehicleController carController;

    private bool isFirePlaying = true;

    private void Awake()
    {
        fire = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        bool shouldPlay = carController.carVelocity.magnitude >= 200f;

        if (shouldPlay && !isFirePlaying)
        {
            fire.Play();
            isFirePlaying = true;
        }
        else if (!shouldPlay && isFirePlaying)
        {
            fire.Pause();
            fire.Clear();
            isFirePlaying = false;
        }
    }
}
