using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeToLobby : MonoBehaviour
{
    public PlayableDirector EndingScene; // 타임라인

    void Start()
    {
        if (EndingScene != null)
        {
            EndingScene.stopped += OnTimelineEnd; // 타임라인 종료 시 이벤트 실행
        }
    }

    void OnTimelineEnd(PlayableDirector director)
    {
        if (director == EndingScene)
        {
            SceneManager.LoadScene("Lobby"); // 전환할 씬 이름 입력
        }
    }
}
