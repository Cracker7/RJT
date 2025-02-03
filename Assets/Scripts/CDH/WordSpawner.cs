using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WordSpawner : MonoBehaviour
{
    public GameObject wordPrefab; // 단어 프리팹 (TextMeshProUGUI가 포함된 오브젝트)
    public float spawnInterval = 2f; // 단어가 생성되는 간격
    //public List<string> wordList; // 사용될 단어 목록

    public string[] fallText = { "킹왕짱", "내가 최고", "왼쪽임" };

    private List<GameObject> activeWords = new List<GameObject>(); // 화면에 표시된 단어들

    //public Camera mainCamera;
    public float spawnYOffset = -2f;
    public float minXOffset = -5f;
    public float maxOffset = 5f;
    public float fixedZ = 0f;

    public Canvas canvas;

    void Start()
    {
        InvokeRepeating("SpawnRandomWord", 0f, spawnInterval); // 일정 시간 간격으로 단어를 생성
    }

    // 랜덤 단어를 생성하여 화면에 표시
    void SpawnRandomWord()
    {
        //Vector3 screenTop = new Vector3(Screen.width / 2, Screen.height, 10f);
        //Vector3 worldTop = Camera.main.ScreenToWorldPoint(screenTop);

        //float randomX = Random.Range(minXOffset, maxOffset);
        //float spawnY = worldTop.y + spawnYOffset;

       // Vector3 spawnPosition = new Vector3(randomX, spawnY, fixedZ);

        string randomWord = fallText[Random.Range(0, fallText.Length)]; // 랜덤 단어 선택
        //GameObject newWord = Instantiate(wordPrefab, new Vector3(Random.Range(300f,900f), 920f, 0f), Quaternion.identity); // 화면 상단에서 생성


        GameObject newWord = Instantiate(wordPrefab, canvas.transform);
        newWord.transform.localPosition = new Vector3(Random.Range(-830f, 450f), 380f, 0f);

        newWord.GetComponent<TextMeshProUGUI>().text = randomWord; // 텍스트 설정
        activeWords.Add(newWord); // 생성된 단어를 리스트에 추가
    }

    // 화면에서 일치하는 단어를 제거하는 메서드
    public void RemoveWord(string word)
    {
        for (int i = 0; i < activeWords.Count; i++)
        {
            GameObject activeWord = activeWords[i];
            if (activeWord != null && activeWord.GetComponent<TextMeshProUGUI>().text == word)
            {
                activeWords.RemoveAt(i); // 단어를 리스트에서 제거
                Destroy(activeWord); // 해당 단어 제거
                if(word == "좌회전")
                {

                }
                break;
            }
        }
    }
}
