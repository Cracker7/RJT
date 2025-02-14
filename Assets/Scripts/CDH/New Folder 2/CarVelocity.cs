using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarVelocity : MonoBehaviour
{
    public TextMeshProUGUI speedText;  // 속도를 표시할 UI Text
    public PlayerKMS playerKMS;

    public Rigidbody rb;
    public RectTransform needle;  // 다이얼 형태일 경우 바늘을 움직이기 위한 RectTransform
    public float maxSpeed = 100f;  // 최대 속도 제한


    //private void Start()
    //{
    //    rb = playerKMS.currentObjectPrefab.GetComponent<Rigidbody>();
    //}

    private void Update()
    {
        if(playerKMS.currentObjectPrefab != null)
        {
            rb = playerKMS.currentObjectPrefab.GetComponent<Rigidbody>();
        }

        float speed = rb.linearVelocity.magnitude;
        Debug.Log("스피드" + speed);
        //CV.UpdateSpeed(speed)
        speedText.text = "Speed: " + speed.ToString("F2") + " km/h";

        float angle = Mathf.Lerp(0, 180, speed / maxSpeed);
        needle.localRotation = Quaternion.Euler(0, 0, -angle);  // 바늘 회전

    }
    //public void UpdateSpeed(float speed)
    //{
    //    // 텍스트로 속도 표시

    //    // 속도계를 바늘 형태로 구현할 경우 바늘 각도 계산
    //    //float angle = Mathf.Lerp(0, 180, speed / maxSpeed);
    //    //needle.localRotation = Quaternion.Euler(0, 0, -angle);  // 바늘 회전
    //}
}

