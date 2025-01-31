using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldEnter : MonoBehaviour
{
    //public InputField inputField; // InputField UI를 참조할 변수
    public TMP_InputField inputField;
    //public hancom_fallingText HCFT;

    void Start()
    {
        // InputField에서 텍스트 입력 완료 후 엔터가 눌렸을 때 처리할 메서드 연결
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    // Enter 키를 눌렀을 때 호출될 메서드
    void OnEndEdit(string input)
    {
        // 입력된 값이 비어 있지 않으면
        if (!string.IsNullOrEmpty(input))
        {
            // Enter 키를 눌렀을 때 처리할 코드
            Debug.Log("입력된 텍스트: " + input);
            inputField.text = "";
        }
    }


}
