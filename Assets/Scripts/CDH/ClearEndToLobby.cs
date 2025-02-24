using UnityEngine;

public class ClearEndToLobby : MonoBehaviour
{
    public GameObject clearEndImage;  // Main에서 게임 클리어 후 Lobby로 돌아오면 띄 울 이미지

    void Start()
    {
        // 게임 클리어 여부 확인
        if (PlayerPrefs.GetInt("GameCleared", 0) == 1)
        {
            // 게임 클리어가 됐다면 이미지 활성화
            clearEndImage.SetActive(true);
        }
        else
        {
            // 게임 클리어가 안됐다면 이미지 비활성화
            clearEndImage.SetActive(false);
        }
    }
}
