using UnityEngine;

public class RGTCarDownV2 : MonoBehaviour
{
    //빠지는 만큼
    [SerializeField] private float sinkDepth = 2f; 
    //빠지는 속도
    [SerializeField] private float sinkSpeed = 1f; 
    //연타 간격
    [SerializeField] private float requiredTapSpeed = 0.5f; 

    //위치 저장
    private Vector3 startPosition;
    private bool isSinking = true;
    private bool isRising = false;
    private float lastTapTime = 0f;
    private float riseTimer = 0f;


    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //만약에 현재의 맵은 사막맵이고 속도가 60아래 떨어지면 isSinking = true; 나중에 조건을 추가해야되고 위에 isSinking false로 바꿔야 한다.
        if(isSinking)
        {

            SinkSand();

        }
        //연타 검사
        KeepTheKey();

        if (isRising)
        {

            SandUp();

        }

    }


    private void SinkSand()
    {

        float targetY = startPosition.y - sinkDepth;
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), Time.deltaTime * sinkSpeed);
      
    }

    private void SandUp()
    {
        transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * sinkSpeed);

        //목표 위치에 도달하면 멈춤
        if (Vector3.Distance(transform.position, startPosition) < 0.01f)
        {
            isRising = false;
            //테스트할 때 이렇게 설정 나중에 지워야 한다.
            isSinking = false;
        }
    }

    private void KeepTheKey()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //속도 체크
            if (Time.time - lastTapTime < requiredTapSpeed) 
            {
                isRising = true;
                isSinking = false;
                riseTimer = 1f;
            }
            lastTapTime = Time.time;
        }
        
        if(isRising)
        {
            riseTimer -= Time.deltaTime;
            if(riseTimer <= 0)
            {
                isRising = false;
                isSinking = true;
            }
        }
    }

}
