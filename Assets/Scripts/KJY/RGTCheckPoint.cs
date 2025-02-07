using UnityEngine;

public class RGTCheckPoint : MonoBehaviour
{
    //고급스러운 코드만들고 싶다..... 
    //고정적으로 처음 시작하는 위치를 저장한다.
    //결과를 받아 온다. -> 만약에 실패한 결과를 전달받았다. 처음부터 다시게임 시작한다.  Delegate??
    //다시 시작하는 형식 - 저장했던 시작위치에서 플레이어 생성하고 탈것하나 랜덤으로(아니면 특정한 탈것) 생성
    //게임 맨 시작할 때 플레이어 생성하는 코드를 호출하면 되지 않을까??? 없으면 게임 시작할 때 플레이어 생성하는 거랑 오브젝트들 같이 생성하는 것들 같이 만들어야 하지 않을까????

    //시작한 위치
    private Vector3 TheStartPpos;


    private void Update()
    {

    }



}
