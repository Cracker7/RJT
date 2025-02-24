using UnityEngine;

public class RGTForestMapManager : MonoBehaviour
{
    public PlayerKMS player;

    [SerializeField] private GameObject ForestMap;



    //SkyBox
    [SerializeField] private Material newSkybox;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color MorningColor = new Color(1.0f, 0.93f, 0.8f);
    [SerializeField] private float MorningIntensity = 1.5f;
    [SerializeField] private Vector3 MorningRotation = new Vector3(30f, 200f, 0f);
    [SerializeField] private float transitionSpeed = 1.5f; // 부드러운 전환 속도

    private Material defaultSkybox;
    private Color defaultLightColor;
    private float defaultLightIntensity;
    private Quaternion defaultLightRotation;
    private bool isInZone = false;
    private float blendFactor = 0f;




    private void Start()
    {
        //SkyBox
        defaultSkybox = RenderSettings.skybox;

        if (directionalLight)
        {
            defaultLightColor = directionalLight.color;
            defaultLightIntensity = directionalLight.intensity;
            defaultLightRotation = directionalLight.transform.rotation;
        }
    }


    private void Update()
    {
        if (player.currentState == PlayerKMS.PlayerState.Dead)
        {
            ChangeTheSkyBox();
            ForestMap.SetActive(true);
        }

        if (isInZone)
        {
            blendFactor = Mathf.Lerp(blendFactor, 1f, Time.deltaTime * transitionSpeed);
        }
        else
        {
            blendFactor = Mathf.Lerp(blendFactor, 0f, Time.deltaTime * transitionSpeed);
        }

        // 조명 전환
        if (directionalLight)
        {
            directionalLight.color = Color.Lerp(defaultLightColor, MorningColor, blendFactor);
            directionalLight.intensity = Mathf.Lerp(defaultLightIntensity, MorningIntensity, blendFactor);

            Quaternion targetRotation = Quaternion.Euler(MorningRotation);
            directionalLight.transform.rotation = Quaternion.Lerp(defaultLightRotation, targetRotation, blendFactor);
        }
    }


    private void ChangeTheSkyBox()
    {
        RenderSettings.skybox = newSkybox;
        DynamicGI.UpdateEnvironment();
        isInZone = true;
    }
}
