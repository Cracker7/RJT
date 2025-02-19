using UnityEngine;

public class RGTFog : MonoBehaviour
{
    public Color fogColor = new Color(0.6f, 0.4f, 0.2f, 1f); // 갈색 안개
    public float targetFogDensity = 0.03f; // 목표 안개 농도
    public float transitionSpeed = 1.5f; // 부드러운 전환 속도

    private Color defaultFogColor;
    private float defaultFogDensity;
    private bool isInFogZone = false;

    void Start()
    {
        //defaultFogColor = RenderSettings.fogColor;
        //defaultFogDensity = RenderSettings.fogDensity;
        RenderSettings.fog = false;
    }

    void Update()
    {
        if (isInFogZone)
        {
            // 부드럽게 안개 변경
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogColor, Time.deltaTime * transitionSpeed);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime * transitionSpeed);
        }
        else
        {
            // 기본 상태로 복귀
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, defaultFogColor, Time.deltaTime * transitionSpeed);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, defaultFogDensity, Time.deltaTime * transitionSpeed);

            if (Mathf.Abs(RenderSettings.fogDensity - defaultFogDensity) < 0.001f)
            {
                RenderSettings.fog = defaultFogDensity > 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            isInFogZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInFogZone = false;
        }
    }
}
