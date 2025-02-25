using UnityEngine;
using UnityEngine.SceneManagement;


public class CheatKeyToEnd : MonoBehaviour
{
    //public GameObject EndCutScene;
    //public GameObject MainUI;
    //public GameObject SnowMap;
    //public GameObject DesertMap;
    //public GameObject ForestMap;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            //EndCutScene.SetActive(true);
            //MainUI.SetActive(false);
            //SnowMap.SetActive(false);
            //DesertMap.SetActive(false);
            //ForestMap.SetActive(false);

            PlayerPrefs.SetInt("GameCleared", 1); //엔딩 본 후 UI변경을 위해서 저장
            PlayerPrefs.Save(); // 저장

            SceneManager.LoadScene("EndScene");

        }

    }
}
