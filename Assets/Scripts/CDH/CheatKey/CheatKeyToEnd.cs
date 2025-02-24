using UnityEngine;

public class CheatKeyToEnd : MonoBehaviour
{
    public GameObject EndCutScene;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            EndCutScene.SetActive(true);

            PlayerPrefs.SetInt("GameCleared", 1); //엔딩 본 후 UI변경을 위해서 저장
            PlayerPrefs.Save(); // 저장
        }
            
    }
}
