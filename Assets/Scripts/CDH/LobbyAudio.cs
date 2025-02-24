using UnityEngine;

public class LobbyAudio : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip GoMain;
    public AudioClip ExitGame;

    public void ClickMainButton()
    {
        audiosource.clip = GoMain;
        audiosource.Play();
    }

    public void ClickExitButton()
    {
        audiosource.clip = ExitGame;
        audiosource.Play();
    }
}
