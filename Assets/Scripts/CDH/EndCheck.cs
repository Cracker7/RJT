using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCheck : MonoBehaviour
{
    //public GameObject EndCutScene;
    //public GameObject MainUI;
    //public GameObject SnowMap;

    //public GameObject UIs; // 게임 실행 중 뜨는 UI들 모음

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "carbody")
        {
            //EndCutScene.SetActive(true);
            //MainUI.SetActive(false);
            //SnowMap.SetActive(false);

            //// UIs.SetActive(false); // 엔딩 컷씬 동안 UI 끄기

            PlayerPrefs.SetInt("GameCleared", 1); //엔딩 본 후 UI변경을 위해서 저장
            PlayerPrefs.Save(); // 저장

            SceneManager.LoadScene("EndScene");

        }
    }
}
