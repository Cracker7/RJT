using TMPro;
using UnityEngine;
using System.Collections;

public class SpeedUIController : MonoBehaviour
{
    [SerializeField] private RGTPoopGaugeManager poo;
    private GameObject player = null;
    private PlayerKMS playerKMS = null;

    // 속도 표시용 TMP (예: UI 상단에 속도값을 표시)
    public TextMeshProUGUI tmp = null;
    // 카운트다운 표시용 TMP (예: 화면 중앙에 "3", "2", "1" 표시)
    public TextMeshProUGUI countdownTMP = null;

    public float dangerSpeed = 60f;
    private Color currentColor;

    private bool isCountdownActive = false;
    private Coroutine countdownCoroutine = null;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        //Debug.Log("Player : " + player);
        if (player != null)
        {
            //Debug.Log("플레이어 찾음");
            playerKMS = player.GetComponent<PlayerKMS>();
            if(playerKMS != null)
            {
                //Debug.Log("플레이어스크립트 찾음");
            }
        }
        //else
        //{
        //    Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
        //}
    }

    private void Update()
    {
        //Debug.Log("스피드 UI 업데이트 되고 있음");
        // PlayerKMS가 정상적으로 할당되었는지 확인
        if (playerKMS == null)
        {
            //Debug.Log("플레이어를 못찾음");
            return;
        }

        //Debug.Log("스피드 UI에서 player를 찾음");

        // ActiveRigidbody가 탑승 오브젝트의 리지드바디가 있으면 그걸, 없으면 플레이어 자신의 리지드바디(mainRigidbody)를 사용
        float speed = playerKMS.ActiveRigidbody.linearVelocity.magnitude;
        poo.UpdatePoopGauge(speed);
        tmp.text = speed.ToString("F2");

        // 속도에 따라 색상을 변경 (불필요한 업데이트 방지)
        Color newColor = speed <= dangerSpeed ? Color.red : Color.white;
        if (currentColor != newColor)
        {
            tmp.color = newColor;
            currentColor = newColor;
        }
        //Debug.Log("스피드 텍스트 업데이트 완료");

        // 속도가 dangerSpeed 이하이면 카운트다운 시작
        if (speed <= dangerSpeed)
        {
            if (!isCountdownActive && !(playerKMS.currentState == PlayerKMS.PlayerState.Dead))
            {
                countdownCoroutine = StartCoroutine(CountdownToDeath());
            }
        }
        // 속도가 dangerSpeed보다 높으면 카운트다운 중단 및 초기화
        else
        {
            if (isCountdownActive)
            {
                StopCoroutine(countdownCoroutine);
                isCountdownActive = false;
                countdownTMP.text = "";
            }
        }
    }

    private IEnumerator CountdownToDeath()
    {
        isCountdownActive = true;
        // 카운트다운: 3, 2, 1 (1초 간격)
        for (int i = 5; i > 0; i--)
        {
            countdownTMP.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownTMP.text = "";

        // 3초가 지나면 플레이어 상태를 Dead로 변경
        // PlayerKMS에 public 메서드 SetDeadState() 또는 Die()가 있어야 합니다.
        playerKMS.SetDeadState();

        isCountdownActive = false;
    }


    // 속도 받아와서 UI에 업데이트

    // 속도가 일정 이하로 내려가면 글씨가 빨간색으로 바뀜

    // 카운트 다운?( 소리로 째깍째깍 소리를 낸다던지해서 속도가 느려지면 위험하다는걸 표시)
}
