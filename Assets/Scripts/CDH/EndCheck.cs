using UnityEngine;

public class EndCheck : MonoBehaviour
{
    public GameObject EndCutScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "carbody")
        {
            EndCutScene.SetActive(true);

            PlayerPrefs.SetInt("GameCleared", 1); //엔딩 본 후 UI변경을 위해서 저장
            PlayerPrefs.Save(); // 저장
        }
    }
}
