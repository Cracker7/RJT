using UnityEngine;

public class RGTSnowMapManager : MonoBehaviour
{
    [SerializeField] private GameObject ForestMap;
    [SerializeField] private GameObject DesertMap;
    //[SerializeField] private GameObject SnowMap;



    //SkyBox
    [SerializeField] private Material newSkybox;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color NightColor = new Color(0.6f, 0.45f, 0.8f);
    [SerializeField] private float NightIntensity = 1.2f;
    [SerializeField] private Vector3 NightRotation = new Vector3(30f, 200f, 0f);
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
        //SkyBox
        if (isInZone)
        {
            blendFactor = Mathf.Lerp(blendFactor, 1f, Time.deltaTime * transitionSpeed);
        }
        else
        {
            blendFactor = Mathf.Lerp(blendFactor, 0f, Time.deltaTime * transitionSpeed);
        }

        // Skybox 전환
        //RenderSettings.skybox.Lerp(defaultSkybox, newSkybox, blendFactor);
        //DynamicGI.UpdateEnvironment();

        // 조명 전환
        if (directionalLight)
        {
            directionalLight.color = Color.Lerp(defaultLightColor, NightColor, blendFactor);
            directionalLight.intensity = Mathf.Lerp(defaultLightIntensity, NightIntensity, blendFactor);

            Quaternion targetRotation = Quaternion.Euler(NightRotation);
            directionalLight.transform.rotation = Quaternion.Lerp(defaultLightRotation, targetRotation, blendFactor);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            ForestMap.SetActive(false);
            DesertMap.SetActive(false);
            //SnowMap.SetActive(true);

            //SkyBox
            RenderSettings.skybox = newSkybox;
            DynamicGI.UpdateEnvironment();
            isInZone = true;
        }
    }



}
