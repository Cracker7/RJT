using UnityEngine;

public class ClearEndToLobby : MonoBehaviour
{
    public GameObject clearEnd;  // 게임 클리어 이미지(3D 오브젝트)

    void Start()
    {
        // 게임 클리어 여부 확인
        if (PlayerPrefs.GetInt("GameCleared", 0) == 1)
        {
            // 게임 클리어 이미지 활성화
            clearEnd.SetActive(true);
        }
        else
        {
            // 게임 클리어 이미지 비활성화
            clearEnd.SetActive(false);
        }
    }
}
