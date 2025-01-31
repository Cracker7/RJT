using UnityEngine;

public class Word : MonoBehaviour
{
    public float fallSpeed = 2f; // 단어가 떨어지는 속도

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime); // 단어가 아래로 떨어짐

        // 화면 밖으로 나가면 단어 제거
        if (transform.position.y < 100f)
        {
            Destroy(gameObject);
        }
    }
}
