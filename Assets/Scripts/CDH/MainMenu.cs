using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위한 네임스페이스 추가

public class MainMenu : MonoBehaviour
{
    // 첫 번째 버튼 클릭 시 메인 씬으로 이동
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // MainScene은 메인 씬 이름
    }

    // 두 번째 버튼 클릭 시 게임 종료
    public void QuitGame()
    {
#if UNITY_EDITOR // 유니티에디터 실행 도중에 작동되는 함수
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // 빌드된 게임에서 작동되는 함수
#endif
    }
}
