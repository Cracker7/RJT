using UnityEngine;
using System; // Enum 관련 기능 사용
using TMPro;


public class sliderM : MonoBehaviour
{
    // 상태를 나타내는 enum 선언 (태그 이름은 소문자로 사용하므로 ToLower()로 변환)
    public enum CollisionState
    {
        Win,
        Fail,
        Pass
    }

    public RectTransform handle;
    private float moveSpot;            // 핸들의 이동 위치를 저장하는 변수
    private bool movingRight = true;   // 핸들이 오른쪽으로 이동 중인지 여부
    private bool isPaused = false;     // 핸들의 이동이 일시 중지되었는지 여부
    public Canvas canvas;
    public CollisionState lastCollisionState = CollisionState.Fail;
    public static event Action OnShutdown;
    int seconds;
    int milliseconds;

    //public TextMeshProUGUI stopwatchText; // UI Text에 시간을 표시할 변수
    private float elapsedTime; // 경과 시간

    private void Start()
    {
        moveSpot = handle.anchoredPosition.x; // 핸들의 초기 위치 저장
        elapsedTime = 0f;
    }

    private void Update()
    {
       // if (!isPaused) elapsedTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        

        HandleInput(); // 사용자 입력 처리
        if (!isPaused) // 이동이 일시 중지가 아닐 때 , isPaused = false;
        {
            MoveHandle(); // 핸들 이동
        }

        
    }

 
    // 스톱워치 시간 표시 업데이트
    private void UpdateStopwatchDisplay()
    {
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100); 
        //stopwatchText.text = seconds.ToString() + ":" + milliseconds.ToString();
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = true;
            if (isPaused) // 이동이 일시 중지되었을 때
            {
                lastCollisionState = CheckCollision();  // 충돌 체크 (내부에서 상태에 따라 처리)
                //Invoke("ShutDown"); // 1초 후에 canvas 끄기
                ShutDown();
            }
        }
    }

    private void MoveHandle()
    {
        // Time.deltaTime과 Time.timeScale을 이용하여 이동 속도 결정
        //float moveSpeed = Time.deltaTime * 50;
        //moveSpot += movingRight ? moveSpeed : -moveSpeed; // 이동 방향에 따라 좌표 증가/감소
        moveSpot += Time.deltaTime * 100;


        // 좌우 한계점 체크 (0 ~ 145)
        if (moveSpot >= 145)
        {
            lastCollisionState = CollisionState.Fail; // Fail 결과 설정
            ShutDown();
            Debug.Log("Fail결과 여기에 넣기");

        }


        // 핸들의 위치 업데이트
        handle.anchoredPosition = new Vector2(moveSpot, handle.anchoredPosition.y);
    }

    /// <summary>
    /// 충돌을 체크하고, 충돌한 상태에 따라 로그를 출력합니다.
    /// </summary>
    public CollisionState CheckCollision()
    {
        string[] tags = { "win", "fail", "pass" };
        foreach (string tag in tags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag); // 해당 태그를 가진 객체들을 찾음
            foreach (GameObject obj in objects)
            {
                RectTransform rt = obj.GetComponent<RectTransform>(); // 객체의 RectTransform을 가져옴
                if (RectTransformUtility.RectangleContainsScreenPoint(rt, handle.position)) // 핸들이 객체 위에 있는지 확인
                {
                    // Debug.Log($"Handle is on {tag} "); // 핸들이 해당 태그 위에 있음을 로그에 출력
                    switch (tag)
                    {
                        case "win":
                            Debug.Log("win");
                            return CollisionState.Win;
                        case "fail":
                            Debug.Log("fail");
                            return CollisionState.Fail;
                        case "pass":
                            Debug.Log("pass");
                            return CollisionState.Pass;

                    }

                }
            }
        }
        return CollisionState.Fail;
    }

    private void ShutDown()
    {
       // elapsedTime = 0f;
        isPaused = true;
        moveSpot = 0f;
        canvas.gameObject.SetActive(false);
        OnShutdown?.Invoke();
    }

    // 나중에 다시 시작할 때 사용할 함수
    public void OpenCanvas()
    {
       // elapsedTime = 0f;
        canvas.gameObject.SetActive(true);
        isPaused = false;

    }
   
}
