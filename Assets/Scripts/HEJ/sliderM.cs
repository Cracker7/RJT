using UnityEngine;

public class sliderM : MonoBehaviour
{
    public RectTransform handle; // UI 핸들을 참조하는 RectTransform
    private float moveSpot; // 핸들의 이동 위치를 저장하는 변수
    private bool movingRight = true; // 핸들이 오른쪽으로 이동 중인지 여부를 나타내는 변수
    private bool isPaused = false; // 핸들의 이동이 일시 중지되었는지 여부를 나타내는 변수
    public Canvas canvas;

    private void Start()
    {
        moveSpot = handle.anchoredPosition.x; // 핸들의 초기 위치를 moveSpot 변수에 저장
    }

    private void Update()
    {
        HandleInput(); // 사용자 입력을 처리
        if (!isPaused) // 이동이 일시 중지되지 않은 경우
        {
            MoveHandle(); // 핸들을 이동
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누르면
        {
            isPaused = !isPaused; // 이동 상태를 토글
            if (isPaused) // 이동이 일시 중지된 경우
            {
                CheckCollision(); // 충돌을 확인
            Invoke("ShutDown", 10f);
            }

        }
    }

    private void MoveHandle()
    {
        float moveSpeed = Time.deltaTime * 40; // 이동 속도를 설정
        moveSpot += movingRight ? moveSpeed : -moveSpeed; // 이동 방향에 따라 moveSpot을 증가 또는 감소

        if (moveSpot >= 200) // moveSpot이 140 이상이면
        {
            movingRight = false; // 이동 방향을 왼쪽으로 변경
        }
        else if (moveSpot <= 0) // moveSpot이 0 이하이면
        {
            movingRight = true; // 이동 방향을 오른쪽으로 변경
        }

        handle.anchoredPosition = new Vector2(moveSpot, handle.anchoredPosition.y); // 핸들의 위치를 업데이트
    }

    private void CheckCollision()
    {
        string[] tags = { "grey", "fail", "pass" };
        foreach (string tag in tags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag); // 해당 태그를 가진 객체들을 찾음
            foreach (GameObject obj in objects)
            {
                RectTransform rt = obj.GetComponent<RectTransform>(); // 객체의 RectTransform을 가져옴
                if (RectTransformUtility.RectangleContainsScreenPoint(rt, handle.position)) // 핸들이 객체 위에 있는지 확인
                {
                    Debug.Log($"Handle is on {tag} tag"); // 핸들이 해당 태그 위에 있음을 로그에 출력
                }
            }
        }
    }

    private void ShutDown()
    {
        canvas.gameObject.SetActive(false);
    }

}
