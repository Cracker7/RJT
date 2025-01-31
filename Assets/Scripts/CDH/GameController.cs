using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_InputField inputField; // 입력 필드
    public WordSpawner wordSpawner; // 단어 생성 스크립트

    void Update()
    {
        // Enter 키를 눌렀을 때 입력된 단어 확인
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string input = inputField.text; // 입력된 텍스트

            if (!string.IsNullOrEmpty(input))
            {
                wordSpawner.RemoveWord(input); // 해당 단어 제거
                inputField.Select();  // InputField 선택
                inputField.ActivateInputField();
                inputField.text = ""; // 입력 필드 비우기
            }
        }
    }

    public void hancomKey()
    {

    }
}
