using UnityEngine;
using UnityEngine.UI;

public class RGTPoopGaugeManager : MonoBehaviour
{
    public Image poopFillColorImage;  // 똥 안을 채우는 색 이미지
    private float poopValue = 1f;
    public float decayRate = 0.2f; // 똥이 줄어드는 속도
    private float minSpeed = 100f; // 최대 속도


    public void UpdatePoopGauge(float speed)
    {
        if (speed <= minSpeed)
        {
            // 100 이하일 때 똥이 가득 참
            poopValue = Mathf.MoveTowards(poopValue, 1f, 5f * Time.deltaTime);
        }
        else
        {
            // 100 이상일 때 똥이 서서히 줄어듦
            poopValue = Mathf.MoveTowards(poopValue, 0f, decayRate * Time.deltaTime);
        }

        // UI 반영 (fillAmount 적용)
        poopFillColorImage.fillAmount = poopValue;
    }
}