using UnityEngine;

public class RGTCheckCarPosManager : MonoBehaviour
{
    //위치계산
    [SerializeField] private Transform TheLeftCar;
    [SerializeField] private Transform TheRightCar;
    [SerializeField] private Transform ThePeople;
    //죽을 때 활성화
    [SerializeField] private GameObject RegPeple;
    [SerializeField] private GameObject LCar;
    [SerializeField] private GameObject RCar;
    [SerializeField] private GameObject Remy;
    [SerializeField] private GameObject RC;
    [SerializeField] private GameObject LC;
    //빈값을 따라가기
    [SerializeField] private Transform LHand;
    [SerializeField] private Transform LFoot;
    [SerializeField] private Transform RHand;
    [SerializeField] private Transform RFoot;
    //Hand & Foot
    [SerializeField] private Transform LHandRemy;
    [SerializeField] private Transform LFootRemy;
    [SerializeField] private Transform RHandRemy;
    [SerializeField] private Transform RFootRemy;
    

    private void Update()
    {

        FollowPos();
        //인간하고 차 연결되어 있는 상태 하에 함수 실행 조건 추가
        CheckThePos();
    }


    private void CheckThePos()
    {
        float Threshold = 1.5f;
        float LandC = ThePeople.transform.localPosition.x - TheLeftCar.transform.localPosition.x;
        float RandC = ThePeople.transform.localPosition.x - TheRightCar.transform.localPosition.x;

        if(Mathf.Abs(LandC) >= Threshold || Mathf.Abs(RandC) >= Threshold)
        {
            // 인간하고  분리한다.

            RegPeple.SetActive(true);
            LCar.SetActive(true);
            RCar.SetActive(true);

            Destroy(Remy);
            Destroy(LC);
            Destroy(RC);

            this.enabled = false;
        }


        Debug.Log("L : " + LandC);
        Debug.Log("R : " + RandC);
    }

    private void FollowPos()
    {
        LHandRemy.transform.position = LHand.transform.position;
        //Debug.Log("RemyH " + LHandRemy.transform.position + "LhandC " + LHand.transform.position);
        LFootRemy.transform.position = LFoot.transform.position;
        RFootRemy.transform.position = RFoot.transform.position;
        RHandRemy.transform.position = RHand.transform.position;
    }
}
