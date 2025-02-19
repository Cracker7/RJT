using UnityEngine;

public class RGTBGMmanager : MonoBehaviour
{
    public AudioSource bgmSource;

    void Start()
    {
        if (bgmSource != null)
        {
            bgmSource.loop = true; 
            bgmSource.Play(); 
        }
    }
}
