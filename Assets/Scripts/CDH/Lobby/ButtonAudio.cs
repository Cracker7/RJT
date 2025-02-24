using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource audioSource; // 하나의 오디오 소스 사용
    public AudioClip soundClip; // 버튼별로 다른 소리를 설정

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 버튼 위에 마우스를 가져다 대면 소리가 나옴
        if (audioSource != null && soundClip != null)
        {
            audioSource.clip = soundClip; // 버튼별로 나올 소리가 달라짐
            audioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 버튼 위에서 마우스를 떼면 소리가 멈춤
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
